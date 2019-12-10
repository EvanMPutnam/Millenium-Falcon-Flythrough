﻿using UnityEngine;

public class Generic_Parser {


    private const string X_COL = "x";
    private const string Y_COL = "y";
    private const string Z_COL = "z";
    private const string R_COL = "phot_rp_mean_mag";
    private const string G_COL = "phot_g_mean_mag";
    private const string B_COL = "phot_bp_mean_mag";
    private const string B_MINUS_V = "bp_rp";
    private const string RADIUS = "radius";

    public static StarContainer Create_Star_Cluster(TextAsset file, int mod_val, float scale_val){
        StarContainer s = new StarContainer(mod_val, scale_val);
        string[] lines = file.text.Split('\n');

        //X, Y, Z coordinates for plotting
        int x_index = -1;
        int y_index = -1;
        int z_index = -1;

        //RGB values for display.
        int r_index = -1;
        int g_index = -1;
        int b_index = -1;

        //Radius of the star
        int radius_index = -1;

        //B-V value
        int b_minx_v_index = -1;

        //If rgb values are found
        bool rgb_found = false;

        int count = 0;

        //int total_rgb_vals = 0;

        float max_r_val = 0;
        float max_g_val = 0;
        float max_b_val = 0;

        float max_radius = 0;

        float max_b_minus_v = 0;

        bool first_val = true;
        foreach (string line in lines){
            if(first_val){
                string[] line_parts = line.Split(',');
                for(int i = 0; i < line_parts.Length; i++){
                    line_parts[i] = line_parts[i].Trim();
                    if(line_parts[i].ToLower().Equals(X_COL)){
                        x_index = i;
                    }
                    if(line_parts[i].ToLower().Equals(Y_COL)){
                        y_index = i;
                    }
                    if(line_parts[i].ToLower().Equals(Z_COL)){
                        z_index = i;
                    }
                    if(line_parts[i].ToLower().Equals(R_COL)){
                        r_index = i;
                    }
                    if(line_parts[i].ToLower().Equals(G_COL)){
                        g_index = i;
                    }
                    if(line_parts[i].ToLower().Equals(B_COL)){
                        b_index = i;
                    }
                    if(line_parts[i].ToLower().Equals(B_MINUS_V)){
                        b_minx_v_index = i;
                    }
                    if(line_parts[i].ToLower().Equals(RADIUS)){
                        radius_index = i;
                    }
                }
                first_val = false;
            }else{
                string[] line_parts = line.Split(',');
                if(line_parts.Length != 1 && count % mod_val == 0){
                    ColorIndex ci = null;
                    float b_minus = 4.0f;
                    //If color found then populate it.
                    if(rgb_found){
                        try{
                            ci = new ColorIndex(float.Parse(line_parts[r_index]),
                                        float.Parse(line_parts[g_index]), 
                                        float.Parse(line_parts[b_index]));
                            
                            Debug.Log(line_parts[r_index]);
                            if(float.Parse(line_parts[r_index]) > max_r_val){
                                max_r_val = float.Parse(line_parts[r_index]);
                            }
                            if(float.Parse(line_parts[g_index]) > max_g_val){
                                max_g_val = float.Parse(line_parts[g_index]);
                            }
                            if(float.Parse(line_parts[b_index]) > max_b_val){
                                max_b_val = float.Parse(line_parts[b_index]);
                            }
                        }catch{
                            ci = new ColorIndex(15.0f, 15.0f, 15.0f);
                        }
                        try{
                            b_minus = float.Parse(line_parts[b_minx_v_index]);
                            if(b_minus > max_b_minus_v){
                                max_b_minus_v = b_minus;
                            }
                        }catch{
                            b_minus = 4.0f;
                        }
                    }

                    //Populate star container with a new star. (y,z are flipped for unity.)
                    s.addStar(float.Parse(line_parts[x_index]), 
                                float.Parse(line_parts[z_index]), 
                                float.Parse(line_parts[y_index]), 
                                float.Parse(line_parts[radius_index]),
                                ci, b_minus);
                    if(float.Parse(line_parts[radius_index]) > max_radius){
                        max_radius = float.Parse(line_parts[radius_index]);
                    }
                    
                }
                count += 1;
            }

            //RGB values present
            if(r_index != -1 && g_index != -1 && b_index != -1){
                rgb_found = true;
            }
            //Did not find
            if(x_index == -1 || y_index == -1 || z_index == -1 || radius_index == -1){
                return null;
            }

        }

        s.max_r_val = max_r_val;
        s.max_g_val = max_g_val;
        s.max_b_val = max_b_val;
        s.max_radius = max_radius;

        return s;
    }

}