#!/usr/bin/env bash
python3 manage.py migrate
python3 manage.py loaddata graphqlAPI/fixtures/user.json
python3 manage.py runserver 0.0.0.0:8000
