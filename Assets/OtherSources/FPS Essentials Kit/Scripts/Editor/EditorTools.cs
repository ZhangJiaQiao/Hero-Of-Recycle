 using UnityEngine;
 using UnityEditor;
 using System.Collections.Generic;

 public class EditorTools
 {
	private static List<string> layers;
    private static string[] layerNames;

    public static LayerMask LayerMaskField(string label, LayerMask selected)
	{
    	if (layers == null) 
		{
        	layers = new List<string>();
            layerNames = new string[4];
        }
		else 
		{
            layers.Clear ();
        }
             
        int emptyLayers = 0;
        for (int i = 0; i < 32; i++) 
		{
            string layerName = LayerMask.LayerToName (i);
                 
            if (layerName != "") 
			{ 
                layers.Add (layerName);
            } 
			else
			{
            	emptyLayers++;
            }
        }
             
        if (layerNames.Length != layers.Count)
		{
        	layerNames = new string[layers.Count];
        }

        for (int i=0; i < layerNames.Length; i++) 
			layerNames[i] = layers[i];
         
        selected.value =  EditorGUILayout.MaskField (label, selected.value, layerNames);
        return selected;
     }
 }