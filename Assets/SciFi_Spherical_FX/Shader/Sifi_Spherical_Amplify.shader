// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sifi_Sphere_Amplif"
{
	Properties
	{
		_DisplacementMask("Mask Map", 2D) = "white" {}
		_Animation_speed("Animation Speed", Float) = 1
		[Toggle]_deformation_type("Stretching", Float) = 0
		_deformation_type_Factor("Transition Factor", Float) = 1
		_Shrink_Faces_Amplitude("Shrink Face Factor", Float) = 0
		_NormalPush("Push Mult", Float) = 0.01
		_FrontFace_Diffuse_map("Front Face Map", 2D) = "white" {}
		_BackFace_Diffuse_map("Back Face Map", 2D) = "white" {}
		_OutlineTex("Outline Map", 2D) = "white" {}
		_Outline_Opacity("Outline Opacity", Range( 0 , 100)) = 0
		_Outline_Color("Outline Color Mult", Color) = (0,0,0,0)
		_FrontFace_Intensity("FFace Map", Range( 0 , 5)) = 1
		_FrontFace_Color("FrontFace Color Mult", Color) = (1,1,1,0)
		_BackFace_Intensity("Intensity Mult", Range( 0 , 5)) = 1
		_BackFace_Color("BackFace Color Mult", Color) = (1,1,1,0)
		_Tilin_X("Tilin_X", Float) = 1
		_Tilin_Y("Tilin_Y", Float) = 1
		_CustomTexture("CustomTexture", 2D) = "white" {}
		[Toggle]_MatcapTexture("Matcap/Texture", Float) = 0
		_NormalMap("NormalMap", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 uv2_texcoord2;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv3_texcoord3;
			half ASEVFace : VFACE;
		};

		uniform float _deformation_type_Factor;
		uniform sampler2D _DisplacementMask;
		uniform float _Animation_speed;
		uniform float _Tilin_X;
		uniform float _Tilin_Y;
		uniform float _deformation_type;
		uniform float _NormalPush;
		uniform float _Shrink_Faces_Amplitude;
		uniform sampler2D _NormalMap;
		uniform float _MatcapTexture;
		uniform sampler2D _FrontFace_Diffuse_map;
		uniform float _FrontFace_Intensity;
		uniform float4 _FrontFace_Color;
		uniform sampler2D _OutlineTex;
		uniform float _Outline_Opacity;
		uniform float4 _Outline_Color;
		uniform sampler2D _BackFace_Diffuse_map;
		uniform float _BackFace_Intensity;
		uniform float4 _BackFace_Color;
		uniform sampler2D _CustomTexture;


		float2 MyCustomExpression13( float2 coord , int _deformation_type_Factor )
		{
			// Create transition Coord
			// stretch or not
			float2 transition = float2(1,step(coord.x,_deformation_type_Factor));
			return transition;
		}


		float3 MyCustomExpression5( float3 va , float3 n )
		{
			float3 rejection = va - ( ((dot(va, n))/(dot(n,n)))*n );
			return rejection;
		}


		float2 MyCustomExpression28( float3 vnormal )
		{
			float3 normalVS = normalize(mul(UNITY_MATRIX_MV,vnormal));
			float2 uv_matcap = normalVS.xy * 0.5 + float2(0.5,0.5);
			return uv_matcap;
		}


		float2 MyCustomExpression42( float3 vnormal )
		{
			float3 normalVS = normalize(mul(UNITY_MATRIX_MV,vnormal));
			float2 uv_matcap = normalVS.xy * 0.5 + float2(0.5,0.5);
			return uv_matcap;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float2 coord13 = float2( 0,0 );
			int _deformation_type_Factor13 = (int)_deformation_type_Factor;
			float2 localMyCustomExpression13 = MyCustomExpression13( coord13 , _deformation_type_Factor13 );
			float2 temp_cast_1 = (_Animation_speed).xx;
			float2 appendResult60 = (float2(_Tilin_X , _Tilin_Y));
			float2 panner2 = ( 0.2 * _Time.y * temp_cast_1 + ( appendResult60 * (( _deformation_type )?( v.texcoord.xy ):( v.texcoord1.xy )) ));
			float4 tex2DNode1 = tex2Dlod( _DisplacementMask, float4( panner2, 0, 0.0) );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 va5 = ase_vertex3Pos;
			float3 n5 = ase_vertexNormal;
			float3 localMyCustomExpression5 = MyCustomExpression5( va5 , n5 );
			v.vertex.xyz += ( ( ase_vertexNormal * localMyCustomExpression13.y * tex2DNode1.r * ( _NormalPush * 0.01 ) ) + ( ( localMyCustomExpression5 * float3( -1,-1,-1 ) ) * _Shrink_Faces_Amplitude * tex2DNode1.r * localMyCustomExpression13.y ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = tex2D( _NormalMap, (( _deformation_type )?( i.uv_texcoord ):( i.uv2_texcoord2 )) ).rgb;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 vnormal28 = ase_vertexNormal;
			float2 localMyCustomExpression28 = MyCustomExpression28( vnormal28 );
			float4 tex2DNode27 = tex2D( _FrontFace_Diffuse_map, localMyCustomExpression28 );
			float2 temp_cast_1 = (_Animation_speed).xx;
			float2 appendResult60 = (float2(_Tilin_X , _Tilin_Y));
			float2 panner2 = ( 0.2 * _Time.y * temp_cast_1 + ( appendResult60 * (( _deformation_type )?( i.uv_texcoord ):( i.uv2_texcoord2 )) ));
			float4 tex2DNode1 = tex2D( _DisplacementMask, panner2 );
			float dotResult44 = dot( tex2DNode27 , float4(0.5,0.5,0.5,0.5) );
			float4 temp_output_34_0 = ( tex2D( _OutlineTex, i.uv3_texcoord3 ) * tex2DNode1.r * ( _Outline_Opacity * 0.1 ) * _Outline_Color * dotResult44 );
			float3 vnormal42 = ase_vertexNormal;
			float2 localMyCustomExpression42 = MyCustomExpression42( vnormal42 );
			float4 switchResult53 = (((i.ASEVFace>0)?(( ( tex2DNode27 * _FrontFace_Intensity * _FrontFace_Color ) + temp_output_34_0 )):(( tex2D( _BackFace_Diffuse_map, localMyCustomExpression42 ) * _BackFace_Intensity * _BackFace_Color ))));
			o.Albedo = ( (( _MatcapTexture )?( tex2D( _CustomTexture, (( _deformation_type )?( i.uv_texcoord ):( i.uv2_texcoord2 )) ) ):( switchResult53 )) * 2.5 ).rgb;
			o.Emission = temp_output_34_0.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ScifiFXUI"
}
/*ASEBEGIN
Version=17500
0;11;1920;1007;1342.63;1921.562;1.190431;True;True
Node;AmplifyShaderEditor.CommentaryNode;39;-2100.692,-1838.605;Inherit;False;1560.253;1406.005;FronFace;14;30;33;32;31;35;38;34;44;46;47;48;49;67;68;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;64;-1539.525,722.7207;Inherit;False;2070.935;1955.83;Displacement;26;17;3;63;61;16;60;4;59;2;7;15;6;13;1;5;21;11;8;18;54;25;12;24;23;10;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;30;-1966.444,-1788.605;Inherit;False;869.892;593.0302;Matcap;3;29;28;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1489.525,990.1;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;63;-1195.752,931.7207;Inherit;False;Property;_Tilin_Y;Tilin_Y;16;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1461.199,1483.741;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;-1245.752,772.7207;Inherit;False;Property;_Tilin_X;Tilin_X;15;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;29;-1916.444,-1374.575;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;60;-1049.752,862.7207;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;16;-1187.04,1178.005;Inherit;False;Property;_deformation_type;Stretching;2;0;Create;False;0;0;False;0;0;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-859.7521,865.7207;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CustomExpressionNode;28;-1692.612,-1508.874;Inherit;False;float3 normalVS = normalize(mul(UNITY_MATRIX_MV,vnormal))@$$$float2 uv_matcap = normalVS.xy * 0.5 + float2(0.5,0.5)@$return uv_matcap@;2;False;1;True;vnormal;FLOAT3;0,0,0;In;;Float;False;My Custom Expression;True;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-873.8701,1141.965;Inherit;False;Property;_Animation_speed;Animation Speed;1;0;Create;False;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;40;-2041.092,-339.227;Inherit;False;869.892;593.0302;BackFace;3;43;42;41;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-2050.683,-945.8899;Inherit;False;Property;_Outline_Opacity;Outline Opacity;9;0;Create;False;0;0;False;0;0;0;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1953.515,-836.5762;Inherit;False;Constant;_Float1;Float 1;17;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;2;-614.8326,923.5351;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;27;-1420.552,-1738.605;Inherit;True;Property;_FrontFace_Diffuse_map;Front Face Map;6;0;Create;False;0;0;False;0;-1;76f5071fef6d4634699c3d22ad4fef6c;76f5071fef6d4634699c3d22ad4fef6c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;41;-1991.092,74.80299;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-2072.557,-1122.712;Inherit;False;2;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;46;-1622.895,-674.2153;Inherit;False;Constant;_Vector0;Vector 0;10;0;Create;True;0;0;False;0;0.5,0.5,0.5,0.5;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-1751.515,-936.5762;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;31;-1791.189,-1151.819;Inherit;True;Property;_OutlineTex;Outline Map;8;0;Create;False;0;0;False;0;-1;41b2afa44ca5ae54b9b447c2805d69ab;41b2afa44ca5ae54b9b447c2805d69ab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-1662.968,-842.5117;Inherit;False;Property;_Outline_Color;Outline Color Mult;10;0;Create;False;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-318.6529,896.0651;Inherit;True;Property;_DisplacementMask;Mask Map;0;0;Create;False;0;0;False;0;-1;0fc8bf4d13e7b2c44872d87a42008190;0fc8bf4d13e7b2c44872d87a42008190;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;49;-1080.984,-1602.509;Inherit;False;Property;_FrontFace_Color;FrontFace Color Mult;12;0;Create;False;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-1104.07,-1783.55;Inherit;False;Property;_FrontFace_Intensity;FFace Map;11;0;Create;False;0;0;False;0;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;42;-1767.26,-59.49608;Inherit;False;float3 normalVS = normalize(mul(UNITY_MATRIX_MV,vnormal))@$$$float2 uv_matcap = normalVS.xy * 0.5 + float2(0.5,0.5)@$return uv_matcap@;2;False;1;True;vnormal;FLOAT3;0,0,0;In;;Float;False;My Custom Expression;True;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;44;-1279.038,-694.871;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;7;-903.8327,1619.536;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;52;-1126.804,8.71313;Inherit;False;Property;_BackFace_Color;BackFace Color Mult;14;0;Create;False;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;43;-1495.2,-289.227;Inherit;True;Property;_BackFace_Diffuse_map;Back Face Map;7;0;Create;False;0;0;False;0;-1;d8cfe409d2fb65842a7151f63c8307c5;d8cfe409d2fb65842a7151f63c8307c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;-1156.89,-317.3279;Inherit;False;Property;_BackFace_Intensity;Intensity Mult;13;0;Create;False;0;0;False;0;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-873.2117,-1727.658;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1087.215,-1150.183;Inherit;False;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;6;-911.8327,1453.536;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-1261.077,2563.551;Inherit;False;Property;_deformation_type_Factor;Transition Factor;3;0;Create;False;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-838.0317,-283.4358;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;5;-651.8326,1453.536;Inherit;False;float3 rejection = va - ( ((dot(va, n))/(dot(n,n)))*n )@$return rejection@;3;False;2;True;va;FLOAT3;0,0,0;In;;Float;False;True;n;FLOAT3;0,0,0;In;;Float;False;My Custom Expression;True;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-115.4759,2380.481;Inherit;False;Property;_NormalPush;Push Mult;5;0;Create;False;0;0;False;0;0.01;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-695.7724,-1404.174;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;13;-942.161,2433.832;Inherit;False;// Create transition Coord$// stretch or not$float2 transition = float2(1,step(coord.x,_deformation_type_Factor))@$return transition@$;2;False;2;True;coord;FLOAT2;0,0;In;;Float;False;True;_deformation_type_Factor;INT;0;In;;Float;False;My Custom Expression;True;False;0;2;0;FLOAT2;0,0;False;1;INT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalVertexDataNode;11;-889.8326,2052.535;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-351.8326,1454.536;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;-1,-1,-1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwitchByFaceNode;53;-20.49226,-310.2027;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;70.2749,2381.139;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;18;-533.1109,2430.686;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;25;-301.3302,1608.072;Inherit;False;Property;_Shrink_Faces_Amplitude;Shrink Face Factor;4;0;Create;False;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;69;-72.43053,-1273.182;Inherit;True;Property;_CustomTexture;CustomTexture;17;0;Create;True;0;0;False;0;-1;64e7766099ad46747a07014e44d0aea1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;168.5692,1456.695;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;66;528.17,-185.1307;Inherit;False;Constant;_Float0;Float 0;17;0;Create;True;0;0;False;0;2.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;91.06405,2049.595;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;71;366.5966,-1097.475;Inherit;False;Property;_MatcapTexture;Matcap/Texture;18;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-597.8326,1892.536;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;9;-894.8326,1872.535;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;377.4094,1779.516;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-335.5802,-1248.207;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;686.8616,-304.2581;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;72;403.873,-62.90386;Inherit;True;Property;_NormalMap;NormalMap;19;0;Create;True;0;0;False;0;-1;9a4a55d8d2e54394d97426434477cdcf;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1135.606,-302.8199;Float;False;True;-1;2;ScifiFXUI;0;0;Standard;Sifi_Sphere_Amplif;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;1;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;1;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;60;0;61;0
WireConnection;60;1;63;0
WireConnection;16;0;17;0
WireConnection;16;1;3;0
WireConnection;59;0;60;0
WireConnection;59;1;16;0
WireConnection;28;0;29;0
WireConnection;2;0;59;0
WireConnection;2;2;4;0
WireConnection;27;1;28;0
WireConnection;67;0;35;0
WireConnection;67;1;68;0
WireConnection;31;1;32;0
WireConnection;1;1;2;0
WireConnection;42;0;41;0
WireConnection;44;0;27;0
WireConnection;44;1;46;0
WireConnection;43;1;42;0
WireConnection;47;0;27;0
WireConnection;47;1;48;0
WireConnection;47;2;49;0
WireConnection;34;0;31;0
WireConnection;34;1;1;1
WireConnection;34;2;67;0
WireConnection;34;3;38;0
WireConnection;34;4;44;0
WireConnection;50;0;43;0
WireConnection;50;1;51;0
WireConnection;50;2;52;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;33;0;47;0
WireConnection;33;1;34;0
WireConnection;13;1;15;0
WireConnection;8;0;5;0
WireConnection;53;0;33;0
WireConnection;53;1;50;0
WireConnection;54;0;21;0
WireConnection;18;0;13;0
WireConnection;69;1;16;0
WireConnection;24;0;8;0
WireConnection;24;1;25;0
WireConnection;24;2;1;1
WireConnection;24;3;18;1
WireConnection;12;0;11;0
WireConnection;12;1;18;1
WireConnection;12;2;1;1
WireConnection;12;3;54;0
WireConnection;71;0;53;0
WireConnection;71;1;69;0
WireConnection;23;0;12;0
WireConnection;23;1;24;0
WireConnection;65;0;71;0
WireConnection;65;1;66;0
WireConnection;72;1;16;0
WireConnection;0;0;65;0
WireConnection;0;1;72;0
WireConnection;0;2;34;0
WireConnection;0;11;23;0
ASEEND*/
//CHKSM=CDEE4E52D9F10B01B55155CE1DD2ED38B048ED2B