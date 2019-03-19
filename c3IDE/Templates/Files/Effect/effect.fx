/////////////////////////////////////////////////////////
// {{class}}

//The current foreground texture co-ordinate
varying mediump vec2 vTex;
//The foreground texture sampler, to be sampled at vTex
uniform lowp sampler2D samplerFront;
//The current foreground rectangle being rendered
uniform mediump vec2 srcStart;
uniform mediump vec2 srcEnd;
//The current foreground source rectangle being rendered
uniform mediump vec2 srcOriginStart;
uniform mediump vec2 srcOriginEnd;
//The current foreground source rectangle being rendered, in layout 
uniform mediump vec2 layoutStart;
uniform mediump vec2 layoutEnd;
//The background texture sampler used for background - blending effects
uniform lowp sampler2D samplerBack;
//The current background rectangle being rendered to, in texture co-ordinates, for background-blending effects
uniform mediump vec2 destStart;
uniform mediump vec2 destEnd;
//The time in seconds since the runtime started. This can be used for animated effects
uniform mediump float seconds;
//The size of a texel in the foreground texture in texture co-ordinates
uniform mediump vec2 pixelSize;
//The current layer scale as a factor (i.e. 1 is unscaled)
uniform mediump float layerScale;
//The current layer angle in radians.
uniform mediump float layerAngle;

void main(void)
{
	lowp float a = texture2D(samplerFront, vTex).a;
}


