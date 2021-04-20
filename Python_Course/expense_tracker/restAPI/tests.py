from django.test import TestCase
from restAPI import models


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
