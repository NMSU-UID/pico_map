#!/bin/bash

# Pico map style guidline enforcement should be added here.
# Run install.sh to set it up.

#run python validator
cd ./python/
find ./ -name '*.py' | xargs pep8
cd ./../
