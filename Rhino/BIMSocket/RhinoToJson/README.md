# RhinoToJson
Export Rhino Scene Objects To JSON

# Export plugin for Rhino v5
RhinoToJson plugin export allows you to save parts of your Rhino scene into JSON format. 
JSON is handy when building WebGL based applications, using game engines, such as THREEJS.

# Implementation
Technically this plugin is a very basic implementation of <a href="https://github.com/ubarevicius/RCvA3C">RCvA3C<a> codebase. RCvA3c is Rhinocommon
implementation of <a href="https://github.com/va3c/GHvA3C">GHva3c<a> plugin, which was created to work on Grasshopper 3d (Rhino visual scripting environment).

# Usage
You can download this plugin for free from <a href="http://www.food4rhino.com/app/rhinotojson">food4rhino</a> website. Install via Rhino installer, start Rhino, in command line interface, enter command RhinoToJson.
This will prompt you to select objects you want to export. Afterwards JSON will be generated and you will be offered to save the file. That's it. Enjoy!

# Be aware of dragons
As already mentioned, this is a very basic implementation, which works with basic mesh, brep and surface objects. Curves, polylines and other types of objects were not yet implemented.

# License
RhinoToJson uses MIT license and is opensourced, so contributions are welcome. <a href="https://github.com/ubarevicius/RCvA3C">RCvA3C<a> and <a href="https://github.com/va3c/GHvA3C">GHva3c<a> are also opensourced.