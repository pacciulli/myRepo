from django.test import TestCase
from restAPI import models
from django.urls import reverse
from django.contrib.auth.models import User


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
    def setUp(self):
        user = User.objects.create_user(
            username="test1234", email="test@example.com", password="test1234"
        )
        self.client.login(username="test1234", password="test1234")

    def test_expense_create(self):
        payload = {
            "amount": 50.0,
            "merchant": "AT&T",
            "description": "cell phone subscription",
            "category": "utilities",
        }

        res = self.client.post(
            reverse("restAPI:expense-list-create"), payload, format="json"
        )

        self.assertEqual(res.status_code, 201)

        json_res = res.json()

        self.assertEqual(json_res["amount"], payload["amount"])
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

    def test_expense_create_required_field_missing(self):
        payload = {
            "merchant": "AT&T",
            "description": "cell phone subscription",
            "category": "utilities",
        }

        res = self.client.post(
            reverse("restAPI:expense-list-create"), payload, format="json"
        )

        self.assertEqual(res.status_code, 400)

    def test_expense_retrive(self):
        expense = models.Expense.objects.create(
            amount=300.0, merchant="George", description="loan", category="transfer"
        )
        res = self.client.get(
            reverse("restAPI:expense-retrieve-delete", args=[expense.id]), format="json"
        )

        self.assertEqual(res.status_code, 200)

        json_res = res.json()

        self.assertEqual(json_res["id"], expense.id)
        self.assertEqual(json_res["amount"], expense.amount)
        self.assertEqual(json_res["merchant"], expense.merchant)
        self.assertEqual(json_res["description"], expense.description)
        self.assertEqual(json_res["category"], expense.category)

    def test_expense_delete(self):
        expense = models.Expense.objects.create(
            amount=400.0, merchant="John", description="loan", category="transfer"
        )
        res = self.client.delete(
            reverse("restAPI:expense-retrieve-delete", args=[expense.id]), format="json"
        )

        self.assertEqual(res.status_code, 204)
        self.assertFalse(models.Expense.objects.filter(pk=expense.id).exists())

    def test_list_expense_filter_by_merchant(self):
        amazon_expense = models.Expense.objects.create(
            amount=100.0, merchant="amazon", description="glasses", category="fashion"
        )

        ebay_expense = models.Expense.objects.create(
            amount=200.0, merchant="ebay", description="watch", category="fashion"
        )

        url = "/api/expenses?merchant=amazon"
        res = self.client.get(url, format="json")

        self.assertEqual(res.status_code, 200)

        json_res = res.json()

        self.assertEqual(len(json_res), 1)
        self.assertEqual(json_res[0]["id"], amazon_expense.id)
        self.assertEqual(json_res[0]["amount"], amazon_expense.amount)
        self.assertEqual(json_res[0]["merchant"], amazon_expense.merchant)
        self.assertEqual(json_res[0]["description"], amazon_expense.description)
        self.assertEqual(json_res[0]["category"], amazon_expense.category)
