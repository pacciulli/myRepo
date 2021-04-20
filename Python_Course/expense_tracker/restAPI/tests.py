from django.test import TestCase
from restAPI import models
from django.urls import reverse


# Create your tests here.
class TestModel(TestCase):
    def test_expense(self):
        expense = models.Expense.objects.create(
            amount=249.99,
            merchant="amazon",
            description="anc headphones",
            category="music",
        )
        inserted_expense = models.Expense.objects.get(pk=expense.id)

        self.assertEqual(inserted_expense.amount, 249.99)
        self.assertEqual(inserted_expense.merchant, "amazon")
        self.assertEqual(inserted_expense.description, "anc headphones")
        self.assertEqual(inserted_expense.category, "music")


class TestViews(TestCase):
    def test_expense_create(self):
        payload = {
            "amount": 50,
            "merchant": "AT&T",
            "description": "cell phone subscription",
            "category": "utilities",
        }

        res = self.client.post(
            reverse("restAPI:expense-list-create"), payload, format="json"
        )

        self.assertEqual(res.status_code, 201)

        json_res = res.json()

        self.assertEqual(json_res["amount"], str(payload["amount"]))
        self.assertEqual(json_res["merchant"], payload["merchant"])
        self.assertEqual(json_res["description"], payload["description"])
        self.assertEqual(json_res["category"], payload["category"])
        self.assertIsInstance(json_res["id"], int)

    def test_expense_list(self):
        res = self.client.get(reverse("restAPI:expense-list-create"), format="json")

        self.assertEqual(res.status_code, 200)

        json_res = res.json()

        self.assertIsInstance(json_res, list)

        expenses = models.Expense.objects.all()

        self.assertEqual(len(json_res), len(expenses))
