shader_type canvas_item;

void fragment() {
	vec4 texColor = texture(TEXTURE, UV);
	 COLOR = mix(texColor, vec4(1.0, 1.0, 1.0, texColor.a), step(0.1, 1));
}