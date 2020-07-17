function addControlGui() {

    var colorPicker = new ColourPicker();

    var gui = new dat.GUI();
    // var f1 = gui.addFolder('Configuration');
    var params = {
                intensity: spotLight.intensity,
            };
    gui.add( params, 'intensity', 0, 2 ).onChange( function ( val ) {
                spotLight.intensity = val;
                render();
            } );
  
    gui1_X = gui.add(Object3D, 'Name').listen();

    //Add colour picker
    gui.addColor(colorPicker, 'color0')
    .onChange( function() {  ChangeColor(objectSel, colorPicker.color0); } );

    //Add button to create cubes
    // var obj = { Add_Cube:function(){ ActivateCube(); }};

    // gui.add(obj,'Add_Cube');

}

var ColourPicker = function() {

    this.color0 = "#ffae23"; // CSS string

};
