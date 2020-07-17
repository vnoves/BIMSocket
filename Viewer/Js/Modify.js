

    function moveObj() {
        if (event.code == 'KeyM') {

            if (controlTransform == false){

            control.addEventListener( 'dragging-changed', function ( event ) {
                controls.enabled = ! event.value;
            } );

            control.attach( objectSel.parent );
            
            scene.add( control );
            control.position.set(intersectionPoint.x,intersectionPoint.y,intersectionPoint.z);
            
            
            document.addEventListener( 'keydown', function ( event ) {

                switch ( event.keyCode ) {

                    case 81: // Q
                    control.setSpace( control.space === "local" ? "world" : "local" );
                    break;

                    case 17: // Ctrl
                    control.setTranslationSnap( 100 );
                    control.setRotationSnap( THREE.Math.degToRad( 15 ) );
                    break;

                    case 87: // W
                    control.setMode( "translate" );
                    break;

                    case 69: // E
                    control.setMode( "rotate" );
                    break;

                    case 82: // R
                    control.setMode( "scale" );
                    break;

                    case 187:
                    case 107: // +, =, num+
                    control.setSize( control.size + 0.1 );
                    break;

                    case 189:
                    case 109: // -, _, num-
                    control.setSize( Math.max( control.size - 0.1, 0.1 ) );
                    break;

                    case 88: // X
                    control.showX = ! control.showX;
                    break;

                    case 89: // Y
                    control.showY = ! control.showY;
                    break;

                    case 90: // Z
                    control.showZ = ! control.showZ;
                    break;

                    case 32: // Spacebar
                    control.enabled = ! control.enabled;
                    break;
            }
            }
             ); 
             controlTransform = true;
        }
        else
        {
            DesactivateToggleMove();
        }
        }
    };

   function DesactivateToggleMove(){
    control.removeEventListener( 'dragging-changed', function ( event ) {
            controls.enabled = ! event.value;} );
        control.detach( objectSel.parent );
        scene.remove( control );
        document.removeEventListener( 'keydown', function ( event ) {});
        controlTransform = false;
}
