OfficeMovieService
==================

We like to watch exciting movies at the office. This application helps us to choose them.

Development guide

1. There are two branches in repository: "Development" and "Production". Head of Production is always deployed to AppHarbor.
Process is next: create feature in Development through set of commits; when it's production quality - merge changes
to Production. Do not forget to deploy build.
"master" branch is not used at all. Any changes in domain model / DB must have corresponding data migrations. Data migrations are
applied while application startup.
