using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace ZeldaBossGame
{
    public class SoundManager : GameComponent
    {
        public static string OVERWORLD = "sounds/Termina Field";
        public static string DUNGEON_ONE = "sounds/Middle Boss Battle";
        public static string DUNGEON_TWO = "sounds/Boss Battle";
        public static string DEFEAT_BOSS = "sounds/Boss Clear";
        public static string GAME_OVER = "sounds/Game Over";

        public static string HEART_CONTAINER_CUE_NAME = "Heart_Container_Sound_Effect";
        public static string LINK_ATTACK1 = "MM_Link_Attack1";
        public static string LINK_ATTACK2 = "MM_Link_Attack2";
        public static string LINK_ATTACK3 = "MM_Link_Attack3";
        public static string LINK_ATTACK4 = "MM_Link_Attack4";
        public static string BOSS_DAMAGE = "MM_Gyorg_HitX";
        public static string SWORD1 = "OOT_Sword1";
        public static string LINK_HURT1 = "OOT_YoungLink_Hurt1";
        public static string LINK_HURT2 = "OOT_YoungLink_Hurt2";
        public static string LINK_HURT3 = "OOT_YoungLink_Hurt3";
        public static string ELECTRICITY_SHOCK = "OOT_Link_Shock1";
        public static string ICE_CHARGE = "OOT_Twinrova_Ice_Charge";
        public static string FIREBALL_FLYING = "OOT_PhantomGanon_Ball";
        public static string FIREBALL_HIT = "OOT_PhantomGanon_Ball_Hit";
        public static string FIREBALL_REFLECT = "OOT_PhantomGanon_Ball_Reflect";
        public static string EYEBALL_HIT = "OOT_Stinger_HitX";
        public static string EYEBALL_DIE = "OOT_Stinger_DieX";
        public static string VOLVAGIA_HIT = "OOT_Volvagia_Scream_Battle";
        public static string VOLVAGIA_SWIPE = "OOT_Volvagia_Swipe";
        public static string VOLVAGIA_FLAME = "OOT_KingDodongo_Flame";
        public static string BOULDER_LOOP = "OOT_Boulder_Loop";
        public static string BOULDER_CRASH = "OOT_Boulder_Fall";
        public static string TWINROVA_HIT = "OOT_Twinrova_Comb_HitX";
        public static string TWINROVA_FIRE_GROUND = "OOT_Twinrova_Fire_Ground";
        public static string TWINROVA_ICE_GROUND = "OOT_Twinrova_Ice_Ground";

        AudioEngine audioEngine;
        WaveBank waveBank;
        public SoundBank soundBank;
        public Song overworldSong;
        public Song dungeonSongOne;
        public Song dungeonSongTwo;
        public Song defeatBoss;
        public Song gameOver;

        public SoundManager(Game game) : base(game)
        {
            audioEngine = new AudioEngine(@"Content\sounds\zeldasoundproject.xgs");

            waveBank = new WaveBank(audioEngine, @"Content\sounds\Wave Bank.xwb");

            soundBank = new SoundBank(audioEngine, @"Content\sounds\Sound Bank.xsb");

            overworldSong = Game.Content.Load<Song>(OVERWORLD);

            dungeonSongOne = Game.Content.Load<Song>(DUNGEON_ONE);

            dungeonSongTwo = Game.Content.Load<Song>(DUNGEON_TWO);

            defeatBoss = Game.Content.Load<Song>(DEFEAT_BOSS);

            gameOver = Game.Content.Load<Song>(GAME_OVER);

            MediaPlayer.IsRepeating = true;

            MediaPlayer.Volume = 0.65f;
        }

        public override void Initialize()
        {
            base.Initialize();

            PlayMusic(overworldSong);
        }

        public void PlayCue(string cueName)
        {
            soundBank.GetCue(cueName).Play();
        }

        public void PlayMusic(Song song)
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public void PlayMusicNoLoop(Song song)
        {
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(song);
        }

        public void ResumeMusic()
        {
            MediaPlayer.Resume();
        }

        public void PauseMusic()
        {
            MediaPlayer.Pause();
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
