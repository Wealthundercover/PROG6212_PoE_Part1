# PROG6212_PoE_Part1

# RaceDay - Full-Stack Event Management Platform

## Project Overview
RaceDay is a full-stack event management system designed specifically for the South African road running, walking, and cycling community. It replaces paper-based registrations and spreadsheets with a centralized digital platform for organisers and participants.

## System Roles
The system enforces role-based access control with two primary roles:
* **Organiser:** Can create, edit, and delete events, manage event categories, capture participant results, and view event enrolments.
* **Participant:** Can create an account, browse upcoming events, enter events by selecting categories, view personal enrolments, and track race performance history.

## PoE Part 1 - Documentation & Structure
All planning and database assets are located in the `/docs` folder:
* `erd_diagram.png`: Entity Relationship Diagram (Crow's Foot notation, 6 entities).
* `api_endpoint_plan.md`: Comprehensive RESTful API endpoint specification table.
* `RaceDay_Database_Script.sql`: Complete SQL Server script with schema creation, constraints, and realistic seed data.

## Video Presentation
[Click here to watch the Video Presentation](https://youtube.com/shorts/NRz5mpCMPxc?si=b0Ifq47R1GCRHoBg)

## CI/CD Pipeline
![Build Status](../../workflows/docs-validation.yml/badge.svg)

## References

* **Dahl, R.** (2020) *Building RESTful APIs with C# 9 and .NET 5*. 2nd edn. Birmingham: Packt Publishing.
* **Elmasri, R. and Navathe, S.B.** (2016) *Fundamentals of Database Systems*. 7th edn. Boston: Pearson.
* **GitHub Actions** (2024) *Understanding GitHub Actions*. Available at: https://docs.github.com/en/actions/learn-github-actions/understanding-github-actions (Accessed: 4 September 2026).
* **Microsoft** (2024) *SQL Server Technical Documentation*. Available at: https://learn.microsoft.com/en-us/sql/sql-server/ (Accessed: 4 September 2026).
* **OpenAPI Initiative** (2021) *OpenAPI Specification v3.1.0*. Available at: https://spec.openapis.org/oas/v3.1.0 (Accessed: 4 September 2026).
* **Pressman, R.S. and Maxim, B.R.** (2020) *Software Engineering: A Practitioner's Approach*. 9th edn. New York: McGraw-Hill Education.
