# SAExpiations
 
This is an ASP .NET Expiations Web applicaiton, consisting of four views. This web app allows users to search, filter and select expiations, view said expiations description. Additionally this web app allows users to search, filter, and select locations before viewing the expiations assocciated with said location.
 
## View One: Expiation Code List 
Allows users to see a list of expiation codes, their descriptions and to search for specific codes/descriptions using words like “Exceed” and “fail”; in addition this view allows for filtering based on the number of expiations each code has i.e. only view expiations with a total greater than 0.

## View Two: Expiation Code Details 
Selecting a Expiation Code on the previous page navigates to this page which clearly shows the Expiation Code selected, the description and any other detail for the selected expiation code; This view also allows for year based filtering.

This view loads a monthly breakdown of expiations counts for the selected expiation code, year and grouped by the Notice Status Description.

## View Three: Local Service Area Expiations List 
this view visuaises a List of Local Service Areas (in alphabetical order) and the total number of expiations for Aach area with the option to filter by the selected year

## View Four: Local Service Area Expiations Details
This is the equivalent of the Expiation Codes Detail View except you are displaying the number of expiations for each Expiation Code in the selected Area and year.
