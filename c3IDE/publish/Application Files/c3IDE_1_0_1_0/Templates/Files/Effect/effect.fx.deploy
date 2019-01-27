/////////////////////////////////////////////////////////
// Set color effect
varying mediump vec2 vTex;
uniform lowp sampler2D samplerFront;
uniform lowp vec3 setColor;

void main(void)
{
	lowp float a = texture2D(samplerFront, vTex).a;
	
	gl_FragColor = vec4(setColor.r * a, setColor.g * a, setColor.b * a, a);
}
