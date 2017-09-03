#!/bin/bash

#Install hooks

#install pre-commit hook.
ln -s ../../hooks/validate.sh ./.git/hooks/pre-commit
