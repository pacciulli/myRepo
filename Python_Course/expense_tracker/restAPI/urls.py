from django.urls import path
from restAPI import views


urlpatterns = [
    path("expenses", views.ExpenseListCreate.as_view(), name="expense-list-create"),
    path(
        "expenses/<pk>",
        views.ExpenseRetrieveDelete.as_view(),
        name="expense-retrieve-delete",
    ),
]
