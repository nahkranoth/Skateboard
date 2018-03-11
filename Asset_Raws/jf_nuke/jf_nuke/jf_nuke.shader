//		
//		Icy Hell Skybox By Techx 
//		http://techx.digitalarenas.com
//			11/04/2001
//
// For help with q3map_sun and other shader commands visit this page:
// http://qeradiant.com/manual/Q3AShader_Manual/ch03/pg3_1.htm

// Direction &n elevation checked and adjusted - Speaker

textures/skies/jf_nuke
{
	qer_editorimage env/jf_nuke/nuke_ft.tga
	surfaceparm noimpact
	surfaceparm nolightmap
	q3map_globaltexture
	q3map_lightsubdivide 256
	surfaceparm sky
	q3map_surfacelight 70
	q3map_sun 1 1 1 150 135 70      
        skyparms env/jf_nuke/nuke - -
}
