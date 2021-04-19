from django.test import TestCase
from unittest import TestCase


def two_integers_sum(a, b):
    return a + b


# Create your tests here.
class TestSum(TestCase):
    def test_sum(self):
        self.assertEqual(two_integers_sum(2, 1), 3)
