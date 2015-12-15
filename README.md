# Curacracy

Curacracy is a new kind of art website where content creators and users have the ability to create their own communities all in one place.

## Requirements

* ASP.NET 5
* CoreCLR (Sorry Mono users)
* MVC 6
* EntityFramework 7
* Any CoreCLR supported OS

## Project Breakdown

* CuracracyAPI
  * This is where the real meat of Curacracy resides.  Note: monitisation features are excluded from the open source release.
* CuracracyCommon
  * This is where any common models between the client and API server resides.  Additionally, it includes a basic client API to get you started with creating your own Curacracy apps.
* CuracracyFrontend
  * This is a rudmentary frontend using MVC6 and Razor templates along with CuracracyCommon's client API.

## Building

Building is simple.  From CuracracyAPI and CuracracyFrontend, simply run `dnu restore` then `dnx web`.  Note that for CuracracyAPI an additional step is required before running `dnx web`: you must run `dnx ef migrations add MyFirstMigration` then `dnx ef database update`.

The API uses Sqlite on local machines.  You may access the API and Frontend on ports 5000 and 8080 respectively.
