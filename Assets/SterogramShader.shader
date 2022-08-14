Shader "Custom/SterogramShader" {
	Properties {
		_MainTex ("Depth Map", 2D) = "white" {}
		strips_info ("Strip Count", Vector) = (0.25,0.2,4,0)
		depth_factor ("Depth Factor", Range(-2,2)) = 1.0
		time ("Time", float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment main_frag

			uniform sampler2D _MainTex; // depth map 
										// [1.0/num_strips, 1.0/(num_strips + 1)]
    		uniform float4 strips_info; // depth factor (if negative, invert depth)
			uniform float depth_factor;
			uniform float time;

			struct vert2frag {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};
		
			struct frag2screen {
				float4 color : COLOR;
			};

			struct input {
				 float4 pos : POSITION;
				 half2 uv : TEXCOORD0;
			};

			float random (float2 st) {
				//return st.x;
				float OUT = sin(dot(floor(abs(st.xy * 300.0)),float2(12.9898,78.233)) + time * 100.0)*43758.5453123;
				return abs(OUT) - floor(abs(OUT));
			}

			vert2frag vert(input IN) {
				vert2frag o;
				o.pos = UnityObjectToClipPos(IN.pos);
				o.texcoord = MultiplyUV(UNITY_MATRIX_TEXTURE0, IN.uv);
 
				return o;
			}

			frag2screen main_frag(vert2frag IN)
			{
				frag2screen OUT; 											// texture coordinate from result map
				float2 uv = floor(IN.texcoord.xy * 300) / 300; 								// transform texture coordinate into depth map space 
																			// (removing first strip) and get depth value 
				
				float4 tex = tex2D(_MainTex,uv); 
				
				 // previous strip translated by the displace factor
													// compute displace factor 
																			// (_MainTex_value * factor * strip_width) 
					
					// transform texture coordinate from result map into
				
				for(int i = 0; i < strips_info.z; i++)
				{
					if(uv.x > strips_info.y)
					{
						tex = tex2D(_MainTex,uv);							// if factor negative, invert depth
						if (depth_factor < 0.0)
							tex = 1.0 - tex.x;	
						float a = (uv.x/(strips_info.y - 1.0)) * strips_info.x;
						float displace = a * tex.x * abs(depth_factor) * strips_info.y;
						uv.x = floor((uv.x - strips_info.y + displace) * 300) / 300; 			// assign output color from result map previous strip
					}
				}
				OUT.color = random(uv); 
				return OUT;
			} 
			ENDCG
		}
	}
}
