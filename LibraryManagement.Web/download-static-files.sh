#!/bin/bash

# Create necessary directories
mkdir -p wwwroot/lib/bootstrap/dist/css
mkdir -p wwwroot/lib/bootstrap/dist/js
mkdir -p wwwroot/lib/jquery/dist
mkdir -p wwwroot/lib/bootstrap-icons/font/fonts
mkdir -p wwwroot/lib/jquery-validation/dist
mkdir -p wwwroot/lib/jquery-validation-unobtrusive
mkdir -p wwwroot/css
mkdir -p wwwroot/js

# Download Bootstrap CSS and JS
curl -L -o wwwroot/lib/bootstrap/dist/css/bootstrap.min.css https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css
curl -L -o wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.min.js https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js

# Download jQuery and validation
curl -L -o wwwroot/lib/jquery/dist/jquery.min.js https://code.jquery.com/jquery-3.7.0.min.js
curl -L -o wwwroot/lib/jquery-validation/dist/jquery.validate.min.js https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.5/jquery.validate.min.js
curl -L -o wwwroot/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/4.0.0/jquery.validate.unobtrusive.min.js

# Download Bootstrap Icons
curl -L -o wwwroot/lib/bootstrap-icons/font/bootstrap-icons.css https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css
curl -L -o wwwroot/lib/bootstrap-icons/font/fonts/bootstrap-icons.woff https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/fonts/bootstrap-icons.woff
curl -L -o wwwroot/lib/bootstrap-icons/font/fonts/bootstrap-icons.woff2 https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/fonts/bootstrap-icons.woff2

# Create site.css if it doesn't exist
touch wwwroot/css/site.css

# Create site.js if it doesn't exist
touch wwwroot/js/site.js 