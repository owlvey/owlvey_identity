#!/bin/bash
opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P TheFalcon123 -Q "CREATE DATABASE [FalconAuthDb]"
opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P TheFalcon123 -d FalconAuthDb -i tmp/identity/persistedgrant.sql
opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P TheFalcon123 -d FalconAuthDb -i tmp/identity/configuration.sql
opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P TheFalcon123 -d FalconAuthDb -i tmp/identity/authority.sql

