using System;
using UnityEngine;
using MelonLoader;
using Universal_Unity_ESP;

namespace Warp_the_fight_cheat
{
    public class Warpcheat : MelonMod
    {
        public Rect windowRect = new Rect(20, 20, 250, 400);
        public bool godMode = false;
        public bool inf_Dash = false;
        public bool flying = false;
        public bool fast_fire = false;
        public bool double_dmg = false;
        public bool instakill = false;
        public bool super_fast_fire = false;
        public float speed = 8f;
        public bool tall_model = false;
        public bool tiny_model = false;
        public float jump_height = 2f;
        public bool inf_jump = false;
        public float gravity = -20f;
        public bool noclip = false;
        public bool esp = false;

        //need to implement any vars below this
        public bool disable_blood_effect = false;
        public Player? local_player;

        

        public override void OnGUI()
        {
            windowRect = GUI.Window(420024, windowRect, MakeGuiWork, "Warp the Cheats");
        }

        public void MakeGuiWork(int windowID)
        {
            GUILayout.BeginHorizontal();
            godMode = GUILayout.Toggle(godMode, "God Mode");
            bool heal = GUILayout.Button("Heal");
            GUILayout.EndHorizontal();
            //GUILayout.BeginHorizontal();
            noclip = GUILayout.Toggle(noclip, "Noclip");
            //esp = GUILayout.Toggle(esp, "ESP");
            //GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            inf_Dash = GUILayout.Toggle(inf_Dash, "Infinite Dash");
            speed = GUILayout.HorizontalSlider(speed, 0.0f, 100f);
            GUILayout.Label("Movement Speed (default 8): " + speed.ToString());
            GUILayout.BeginHorizontal();
            flying = GUILayout.Toggle(flying, "Flyhack");
            inf_jump = GUILayout.Toggle(inf_jump, "Infinite Jumps");
            GUILayout.EndHorizontal();
            jump_height = GUILayout.HorizontalSlider(jump_height, 0.0f, 20f);
            GUILayout.Label("Jump Height (default 2): " + jump_height.ToString());
            gravity = GUILayout.HorizontalSlider(gravity, -40f, 20f);
            GUILayout.Label("Gravity (default -20): " + gravity.ToString());
            GUILayout.EndVertical();
            GUILayout.Space(20f);

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            fast_fire = GUILayout.Toggle(fast_fire, "Fire Rate x2");
            super_fast_fire = GUILayout.Toggle(super_fast_fire, "Fire Rate x100");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            double_dmg = GUILayout.Toggle(double_dmg, "Damage x2");
            instakill = GUILayout.Toggle(instakill, "Instakill");
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(20f);

            bool give_score = GUILayout.Button("Give 10 score");

            GUILayout.BeginHorizontal();
            tall_model = GUILayout.Toggle(tall_model, "Tall Model");
            tiny_model = GUILayout.Toggle(tiny_model, "Tiny Model");
            GUILayout.EndHorizontal();

            

            // Not working, I think it has to do with the weird geometry of the game? Not sure really, I tried a few things to fix it, i confirmed im using the correct camera when drawing, But it wont draw correctly no matter what. The ESP will draw sometimes then jump around which is what makes me think the weird maps may be involved.
            /**if (esp)
            {
                foreach (Player player in GameObject.FindObjectsOfType<Player>() as Player[])
                {
                    
                    if (player == local_player)
                    {
                        LoggerInstance.Msg("thats us! skipping");
                        continue;
                    }
                    LoggerInstance.Msg("Found player " + player.NetworkplayerName);
                    LoggerInstance.Msg("Coords: " + player.transform.position.x + ", " + player.transform.position.y + ", " + player.transform.position.z);
                    Vector3 pivotPos = player.transform.position;
                    Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y - 2f;
                    Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y + 2f;

                    Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                    Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);

                    LoggerInstance.Msg("Is w2s z > 0? " + (w2s_footpos.z > 0f));
                    if (true)//w2s_footpos.z > 0f)
                    {
                        DrawBoxESP(w2s_footpos, w2s_headpos, Color.green);
                    }
                }
            }**/

            if (GUI.changed)
            {
                local_player = GameObject.FindObjectOfType<MainCamera>().GetComponentInParent<Player>();
                RigidbodyController p_rigidbody = local_player.gameObject.GetComponent<RigidbodyController>();
                WeaponHandler p_weaponhandler = local_player.gameObject.GetComponent<WeaponHandler>();
                checkDash();
                checkFly();
                checkGodMode();
                checkFastFire();
                checkDmgMulti();
                checkTall();

                p_rigidbody.speed = speed;
                p_rigidbody.jumpHeight = jump_height;
                p_rigidbody.gravity = gravity;
                if (give_score)
                {
                    local_player.score += 10;
                }
                if (heal)
                {
                    local_player.currentHealth = local_player.maxHealth;
                }
                

                if (inf_jump)
                {
                    p_rigidbody.maxDoubleJumps = 1000000000;
                } else
                {
                    p_rigidbody.maxDoubleJumps = 0;
                }
                

                p_rigidbody.GetComponent<Rigidbody>().detectCollisions = !noclip;

                void checkDash()
                {
                    if (inf_Dash)
                    {
                        local_player.maxDashCharge = 10000000;
                        local_player.dashesCharged = (int)local_player.maxDashCharge;
                    }
                    else
                    {
                        local_player.maxDashCharge = 2;
                        if (local_player.dashesCharged > (int)local_player.maxDashCharge)
                        {
                            local_player.dashesCharged = (int)local_player.maxDashCharge;
                        }
                        
                    }
                }

                void checkFly()
                {
                    if (flying)
                    {
                        p_rigidbody.flying = true;
                    }
                    else
                    {
                        p_rigidbody.flying = false;
                    }
                }

                void checkGodMode()
                {
                    if (godMode)
                    {
                        local_player.maxHealth = 100000000000000000;
                        local_player.currentHealth = local_player.maxHealth;
                    }
                    else
                    {
                        local_player.maxHealth = 100;
                        if (local_player.currentHealth > local_player.maxHealth)
                        {
                            local_player.currentHealth = local_player.maxHealth;
                        }
                        
                    }
                }

                void checkFastFire()
                {
                    if (super_fast_fire)
                    {
                        p_weaponhandler.powerUpFirerateMultiplier = 100f;
                    }
                    else if (fast_fire)
                    {
                        p_weaponhandler.powerUpFirerateMultiplier = 2f;
                    } else
                    {
                        p_weaponhandler.powerUpFirerateMultiplier = 1f;
                    }
                }

                void checkDmgMulti()
                {
                    if (instakill)
                    {
                        p_weaponhandler.powerUpDamageMultiplier = 100f;
                    }
                    else if (double_dmg)
                    {
                        p_weaponhandler.powerUpDamageMultiplier = 2f;
                    } else
                    {
                        p_weaponhandler.powerUpDamageMultiplier = 1f;
                    }
                }

                void checkTall()
                {
                    if (tall_model)
                    {
                        p_rigidbody.gameObject.transform.localScale = new Vector3(1, 2, 1);
                    } else if (tiny_model)
                    {
                        p_rigidbody.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    } else
                    {
                        p_rigidbody.gameObject.transform.localScale = Vector3.one;
                    }
                }

            }
        }

        public void DrawBoxESP(Vector3 footpos, Vector3 headpos, Color color)
        {
            float height = headpos.y - footpos.y;
            float widthOffset = 4f;
            float width = height / widthOffset;
            //ESP BOX
            Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - footpos.y - height, width, height, color, 2f);

            //Snapline
            Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), new Vector2(footpos.x, (float)Screen.height - footpos.y), color, 2f);
        }
    }
}
