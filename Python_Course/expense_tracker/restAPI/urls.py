from django.urls import path
from restAPI import views


urlpatterns = [
    path("expenses/", views.ExpenseListCreate.as_view(), name="expense-list-create")
]
