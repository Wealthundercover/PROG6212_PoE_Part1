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
[Click here to watch the Video Presentation]((https://youtube.com/shorts/NRz5mpCMPxc?si=b0Ifq47R1GCRHoBg))

## CI/CD Pipeline
![Build Status](../../workflows/docs-validation.yml/badge.svg)
