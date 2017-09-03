#!/bin/bash

# Pico map style guidline enforcement should be added here.
# TODO: Setup up git hooks to run validation on pre commit

#run python validator
cd ../python/
find ./ -name '*.py' | xargs pep8 -v
cd ../hooks/