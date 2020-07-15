//
// code exported from ThreeNodes.js (github.com/idflood/ThreeNodes.js)
//

require.config({paths: {jQuery: 'loaders/jquery-loader',Underscore: 'loaders/Underscore-loader',Backbone: 'loaders/backbone-loader'}});require(['threenodes/App', 'libs/jquery-1.6.4.min', 'libs/Underscore-min', 'libs/backbone'], function(App) {


var app = new App();
var nodes = app.nodes;

//
// nodes
//

// node: WebGLRenderer
var node_1_data = {
	nid: 1,
	name: 'WebGLRenderer',
	type: 'WebGLRenderer',
	x: 1400,
	y: 300,
	fields: {'in': [
		{name: 'width', val: 800},
		{name: 'height', val: 600},
		{name: 'scene'},
		{name: 'camera'},
		{name: 'bg_color', val: [object Object]},
		{name: 'postfx'},
		{name: 'shadowCameraNear', val: 3},
		{name: 'shadowCameraFar', val: 3000},
		{name: 'shadowMapWidth', val: 512},
		{name: 'shadowMapHeight', val: 512},
		{name: 'shadowMapEnabled'},
		{name: 'shadowMapSoft', val: true},
	]},
	anim: {
	}
};
var node_1 = nodes.createNode(node_1_data);

// node: ParticleSystem
var node_14_data = {
	nid: 14,
	name: 'ParticleSystem',
	type: 'ParticleSystem',
	x: 771,
	y: 156,
	fields: {'in': [
		{name: 'children'},
		{name: 'position', val: [object Object]},
		{name: 'rotation'},
		{name: 'scale', val: [object Object]},
		{name: 'visible', val: true},
		{name: 'castShadow'},
		{name: 'receiveShadow'},
		{name: 'geometry'},
		{name: 'material'},
		{name: 'sortParticles'},
	]},
	anim: {
	}
};
var node_14 = nodes.createNode(node_14_data);

// node: Scene
var node_27_data = {
	nid: 27,
	name: 'Scene',
	type: 'Scene',
	x: 1132,
	y: 170,
	fields: {'in': [
		{name: 'children'},
		{name: 'position', val: [object Object]},
		{name: 'rotation'},
		{name: 'scale', val: [object Object]},
		{name: 'visible', val: true},
		{name: 'castShadow'},
		{name: 'receiveShadow'},
		{name: 'fog'},
	]},
	anim: {
	}
};
var node_27 = nodes.createNode(node_27_data);

// node: Camera
var node_37_data = {
	nid: 37,
	name: 'Camera',
	type: 'Camera',
	x: 908,
	y: 638,
	fields: {'in': [
		{name: 'fov', val: 50},
		{name: 'aspect', val: 1},
		{name: 'near', val: 0.1},
		{name: 'far', val: 2000},
		{name: 'position'},
		{name: 'target', val: [object Object]},
		{name: 'useTarget'},
	]},
	anim: {
	}
};
var node_37 = nodes.createNode(node_37_data);

// node: Merge
var node_47_data = {
	nid: 47,
	name: 'Merge',
	type: 'Merge',
	x: 1001,
	y: 217,
	fields: {'in': [
		{name: 'in0'},
		{name: 'in1'},
		{name: 'in2'},
		{name: 'in3'},
		{name: 'in4'},
		{name: 'in5'},
	]},
	anim: {
	}
};
var node_47 = nodes.createNode(node_47_data);

// node: ParticleBasicMaterial
var node_58_data = {
	nid: 58,
	name: 'ParticleBasicMaterial',
	type: 'ParticleBasicMaterial',
	x: 430,
	y: 194,
	fields: {'in': [
		{name: 'opacity', val: 1},
		{name: 'transparent'},
		{name: 'side'},
		{name: 'depthTest'},
		{name: 'alphaTest'},
		{name: 'polygonOffset'},
		{name: 'polygonOffsetFactor'},
		{name: 'polygonOffsetUnits'},
		{name: 'blending', val: 1},
		{name: 'color'},
		{name: 'map'},
		{name: 'size', val: 30},
		{name: 'sizeAttenuation', val: true},
		{name: 'vertexColors'},
	]},
	anim: {
	}
};
var node_58 = nodes.createNode(node_58_data);

// node: RandomCloudGeometry
var node_73_data = {
	nid: 73,
	name: 'RandomCloudGeometry',
	type: 'RandomCloudGeometry',
	x: 538,
	y: 11,
	fields: {'in': [
		{name: 'nbrParticles', val: 2000},
		{name: 'radius', val: 1000},
		{name: 'rndVelocity'},
		{name: 'linearVelocity'},
	]},
	anim: {
	}
};
var node_73 = nodes.createNode(node_73_data);

// node: Vector3
var node_77_data = {
	nid: 77,
	name: 'Vector3',
	type: 'Vector3',
	x: 698,
	y: 884,
	fields: {'in': [
		{name: 'x'},
		{name: 'y', val: 200},
		{name: 'z', val: 700},
	]},
	anim: {
	}
};
var node_77 = nodes.createNode(node_77_data);

// node: Texture
var node_89_data = {
	nid: 89,
	name: 'Texture',
	type: 'Texture',
	x: 189,
	y: 412,
	fields: {'in': [
		{name: 'image', val: examples/textures/sprites/spark1.png},
	]},
	anim: {
	}
};
var node_89 = nodes.createNode(node_89_data);

// node: Number
var node_94_data = {
	nid: 94,
	name: 'Number',
	type: 'Number',
	x: 180,
	y: 189,
	fields: {'in': [
		{name: 'in'},
	]},
	anim: {
	}
};
var node_94 = nodes.createNode(node_94_data);

// node: Color
var node_98_data = {
	nid: 98,
	name: 'Color',
	type: 'Color',
	x: 208,
	y: 266,
	fields: {'in': [
		{name: 'r', val: 1},
		{name: 'g', val: 1},
		{name: 'b', val: 1},
	]},
	anim: {
	}
};
var node_98 = nodes.createNode(node_98_data);

// node: Number
var node_108_data = {
	nid: 108,
	name: 'Number',
	type: 'Number',
	x: 531,
	y: 479,
	fields: {'in': [
		{name: 'in'},
	]},
	anim: {
	}
};
var node_108 = nodes.createNode(node_108_data);

// node: Number
var node_112_data = {
	nid: 112,
	name: 'Number',
	type: 'Number',
	x: 169,
	y: 154,
	fields: {'in': [
		{name: 'in', val: 1},
	]},
	anim: {
	}
};
var node_112 = nodes.createNode(node_112_data);

// node: Vector3
var node_117_data = {
	nid: 117,
	name: 'Vector3',
	type: 'Vector3',
	x: 427,
	y: 90,
	fields: {'in': [
		{name: 'x', val: 4},
		{name: 'y'},
		{name: 'z'},
	]},
	anim: {
	}
};
var node_117 = nodes.createNode(node_117_data);

// node: Vector3
var node_127_data = {
	nid: 127,
	name: 'Vector3',
	type: 'Vector3',
	x: 426,
	y: 6,
	fields: {'in': [
		{name: 'x', val: 0.3},
		{name: 'y', val: 1},
		{name: 'z', val: 1},
	]},
	anim: {
	}
};
var node_127 = nodes.createNode(node_127_data);

// node: Merge
var node_137_data = {
	nid: 137,
	name: 'Merge',
	type: 'Merge',
	x: 1368,
	y: 757,
	fields: {'in': [
		{name: 'in0'},
		{name: 'in1'},
		{name: 'in2'},
		{name: 'in3'},
		{name: 'in4'},
		{name: 'in5'},
	]},
	anim: {
	}
};
var node_137 = nodes.createNode(node_137_data);

// node: BloomPass
var node_146_data = {
	nid: 146,
	name: 'BloomPass',
	type: 'BloomPass',
	x: 987,
	y: 928,
	fields: {'in': [
		{name: 'strength', val: 2.4},
		{name: 'kernelSize', val: 25},
		{name: 'sigma', val: 4},
		{name: 'resolution', val: 256},
	]},
	anim: {
	}
};
var node_146 = nodes.createNode(node_146_data);

// node: VignettePass
var node_153_data = {
	nid: 153,
	name: 'VignettePass',
	type: 'VignettePass',
	x: 1052,
	y: 1021,
	fields: {'in': [
		{name: 'offset', val: 1.4},
		{name: 'darkness', val: 1.2},
	]},
	anim: {
	}
};
var node_153 = nodes.createNode(node_153_data);

// node: ParticleSystem
var node_158_data = {
	nid: 158,
	name: 'ParticleSystem',
	type: 'ParticleSystem',
	x: 855,
	y: 398,
	fields: {'in': [
		{name: 'children'},
		{name: 'position', val: [object Object]},
		{name: 'rotation'},
		{name: 'scale', val: [object Object]},
		{name: 'visible', val: true},
		{name: 'castShadow'},
		{name: 'receiveShadow'},
		{name: 'geometry'},
		{name: 'material'},
		{name: 'sortParticles'},
	]},
	anim: {
	}
};
var node_158 = nodes.createNode(node_158_data);

// node: RandomCloudGeometry
var node_172_data = {
	nid: 172,
	name: 'RandomCloudGeometry',
	type: 'RandomCloudGeometry',
	x: 530,
	y: 527,
	fields: {'in': [
		{name: 'nbrParticles', val: 2000},
		{name: 'radius', val: 2000},
		{name: 'rndVelocity', val: [object Object]},
		{name: 'linearVelocity', val: [object Object]},
	]},
	anim: {
	}
};
var node_172 = nodes.createNode(node_172_data);

// node: ParticleBasicMaterial
var node_179_data = {
	nid: 179,
	name: 'ParticleBasicMaterial',
	type: 'ParticleBasicMaterial',
	x: 530,
	y: 624,
	fields: {'in': [
		{name: 'opacity', val: 1},
		{name: 'transparent'},
		{name: 'side'},
		{name: 'depthTest'},
		{name: 'alphaTest'},
		{name: 'polygonOffset'},
		{name: 'polygonOffsetFactor'},
		{name: 'polygonOffsetUnits'},
		{name: 'blending', val: 1},
		{name: 'color'},
		{name: 'map'},
		{name: 'size', val: 40},
		{name: 'sizeAttenuation', val: true},
		{name: 'vertexColors'},
	]},
	anim: {
	}
};
var node_179 = nodes.createNode(node_179_data);

// node: Texture
var node_195_data = {
	nid: 195,
	name: 'Texture',
	type: 'Texture',
	x: 287,
	y: 768,
	fields: {'in': [
		{name: 'image', val: examples/textures/sprites/snowflake4.png},
	]},
	anim: {
	}
};
var node_195 = nodes.createNode(node_195_data);

// node: Number
var node_199_data = {
	nid: 199,
	name: 'Number',
	type: 'Number',
	x: 304,
	y: 561,
	fields: {'in': [
		{name: 'in', val: 1},
	]},
	anim: {
	}
};
var node_199 = nodes.createNode(node_199_data);

// node: Number
var node_203_data = {
	nid: 203,
	name: 'Number',
	type: 'Number',
	x: 334,
	y: 626,
	fields: {'in': [
		{name: 'in'},
	]},
	anim: {
	}
};
var node_203 = nodes.createNode(node_203_data);

// node: Color
var node_208_data = {
	nid: 208,
	name: 'Color',
	type: 'Color',
	x: 384,
	y: 694,
	fields: {'in': [
		{name: 'r', val: 0.16862745098039217},
		{name: 'g', val: 0.3333333333333333},
		{name: 'b', val: 0.5803921568627451},
	]},
	anim: {
	}
};
var node_208 = nodes.createNode(node_208_data);

// node: Object3D
var node_218_data = {
	nid: 218,
	name: 'Object3D',
	type: 'Object3D',
	x: 830,
	y: -26,
	fields: {'in': [
		{name: 'children'},
		{name: 'position', val: [object Object]},
		{name: 'rotation'},
		{name: 'scale', val: [object Object]},
		{name: 'visible', val: true},
		{name: 'castShadow'},
		{name: 'receiveShadow'},
	]},
	anim: {
	}
};
var node_218 = nodes.createNode(node_218_data);

//
// connections
//

var connection_46_data = {
	id: 46,
	from_node: 37, from: 'out',
	to_node: 1, to: 'camera'
};
var connection_46 = nodes.createConnectionFromObject(connection_46_data);
var connection_55_data = {
	id: 55,
	from_node: 47, from: 'out',
	to_node: 27, to: 'children'
};
var connection_55 = nodes.createConnectionFromObject(connection_55_data);
var connection_56_data = {
	id: 56,
	from_node: 37, from: 'out',
	to_node: 47, to: 'in5'
};
var connection_56 = nodes.createConnectionFromObject(connection_56_data);
var connection_57_data = {
	id: 57,
	from_node: 14, from: 'out',
	to_node: 47, to: 'in0'
};
var connection_57 = nodes.createConnectionFromObject(connection_57_data);
var connection_86_data = {
	id: 86,
	from_node: 77, from: 'xyz',
	to_node: 37, to: 'position'
};
var connection_86 = nodes.createConnectionFromObject(connection_86_data);
var connection_87_data = {
	id: 87,
	from_node: 73, from: 'out',
	to_node: 14, to: 'geometry'
};
var connection_87 = nodes.createConnectionFromObject(connection_87_data);
var connection_88_data = {
	id: 88,
	from_node: 58, from: 'out',
	to_node: 14, to: 'material'
};
var connection_88 = nodes.createConnectionFromObject(connection_88_data);
var connection_92_data = {
	id: 92,
	from_node: 89, from: 'out',
	to_node: 58, to: 'map'
};
var connection_92 = nodes.createConnectionFromObject(connection_92_data);
var connection_93_data = {
	id: 93,
	from_node: 27, from: 'out',
	to_node: 1, to: 'scene'
};
var connection_93 = nodes.createConnectionFromObject(connection_93_data);
var connection_97_data = {
	id: 97,
	from_node: 94, from: 'out',
	to_node: 58, to: 'depthTest'
};
var connection_97 = nodes.createConnectionFromObject(connection_97_data);
var connection_107_data = {
	id: 107,
	from_node: 98, from: 'rgb',
	to_node: 58, to: 'color'
};
var connection_107 = nodes.createConnectionFromObject(connection_107_data);
var connection_111_data = {
	id: 111,
	from_node: 108, from: 'out',
	to_node: 14, to: 'sortParticles'
};
var connection_111 = nodes.createConnectionFromObject(connection_111_data);
var connection_115_data = {
	id: 115,
	from_node: 112, from: 'out',
	to_node: 58, to: 'transparent'
};
var connection_115 = nodes.createConnectionFromObject(connection_115_data);
var connection_116_data = {
	id: 116,
	from_node: 94, from: 'out',
	to_node: 58, to: 'alphaTest'
};
var connection_116 = nodes.createConnectionFromObject(connection_116_data);
var connection_126_data = {
	id: 126,
	from_node: 117, from: 'xyz',
	to_node: 73, to: 'linearVelocity'
};
var connection_126 = nodes.createConnectionFromObject(connection_126_data);
var connection_136_data = {
	id: 136,
	from_node: 127, from: 'xyz',
	to_node: 73, to: 'rndVelocity'
};
var connection_136 = nodes.createConnectionFromObject(connection_136_data);
var connection_145_data = {
	id: 145,
	from_node: 137, from: 'out',
	to_node: 1, to: 'postfx'
};
var connection_145 = nodes.createConnectionFromObject(connection_145_data);
var connection_152_data = {
	id: 152,
	from_node: 146, from: 'out',
	to_node: 137, to: 'in0'
};
var connection_152 = nodes.createConnectionFromObject(connection_152_data);
var connection_157_data = {
	id: 157,
	from_node: 153, from: 'out',
	to_node: 137, to: 'in1'
};
var connection_157 = nodes.createConnectionFromObject(connection_157_data);
var connection_171_data = {
	id: 171,
	from_node: 158, from: 'out',
	to_node: 47, to: 'in1'
};
var connection_171 = nodes.createConnectionFromObject(connection_171_data);
var connection_178_data = {
	id: 178,
	from_node: 172, from: 'out',
	to_node: 158, to: 'geometry'
};
var connection_178 = nodes.createConnectionFromObject(connection_178_data);
var connection_194_data = {
	id: 194,
	from_node: 179, from: 'out',
	to_node: 158, to: 'material'
};
var connection_194 = nodes.createConnectionFromObject(connection_194_data);
var connection_198_data = {
	id: 198,
	from_node: 195, from: 'out',
	to_node: 179, to: 'map'
};
var connection_198 = nodes.createConnectionFromObject(connection_198_data);
var connection_202_data = {
	id: 202,
	from_node: 199, from: 'out',
	to_node: 179, to: 'transparent'
};
var connection_202 = nodes.createConnectionFromObject(connection_202_data);
var connection_206_data = {
	id: 206,
	from_node: 203, from: 'out',
	to_node: 179, to: 'depthTest'
};
var connection_206 = nodes.createConnectionFromObject(connection_206_data);
var connection_207_data = {
	id: 207,
	from_node: 203, from: 'out',
	to_node: 179, to: 'alphaTest'
};
var connection_207 = nodes.createConnectionFromObject(connection_207_data);
var connection_217_data = {
	id: 217,
	from_node: 208, from: 'rgb',
	to_node: 179, to: 'color'
};
var connection_217 = nodes.createConnectionFromObject(connection_217_data);


// set player mode
app.setDisplayMode('SetDisplayModeCommand', true);
});