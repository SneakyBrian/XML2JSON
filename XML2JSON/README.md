# XML2JSON

XML2JSON is a simple command line tool for converting XML files to JSON. It uses the JSON.NET library.

## Usage:

	XML2JSON.exe input.xml output.json
	
## Example Input:

	<?xml version="1.0"?>
	<catalog>
	   <book id="bk101">
		  <author>Gambardella, Matthew</author>
		  <title>XML Developer's Guide</title>
		  <genre>Computer</genre>
		  <price>44.95</price>
		  <publish_date>2000-10-01</publish_date>
		  <description>An in-depth look at creating applications 
		  with XML.</description>
	   </book>
	   <book id="bk102">
		  <author>Ralls, Kim</author>
		  <title>Midnight Rain</title>
		  <genre>Fantasy</genre>
		  <price>5.95</price>
		  <publish_date>2000-12-16</publish_date>
		  <description>A former architect battles corporate zombies, 
		  an evil sorceress, and her own childhood to become queen 
		  of the world.</description>
	   </book>
	</catalog>
	
## Example Output:

	{
	  "catalog": {
		"book": [
		  {
			"id": "bk101",
			"author": "Gambardella, Matthew",
			"title": "XML Developer's Guide",
			"genre": "Computer",
			"price": "44.95",
			"publish_date": "2000-10-01",
			"description": "An in-depth look at creating applications \r\n      with XML."
		  },
		  {
			"id": "bk102",
			"author": "Ralls, Kim",
			"title": "Midnight Rain",
			"genre": "Fantasy",
			"price": "5.95",
			"publish_date": "2000-12-16",
			"description": "A former architect battles corporate zombies, \r\n      an evil sorceress, and her own childhood to become queen \r\n      of the world."
		  }
		]
	  }
	}
	