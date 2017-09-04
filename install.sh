#!/bin/bash

#Install hooks

#install pre-commit hook.
rm .git/hooks/pre-commit
cp hooks/validate.sh ./.git/hooks/pre-commit
