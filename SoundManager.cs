//-----------------------------------------------------------------------------
// SoundManager maintains a list of cues and their corresponding files.
// This is a very bare bones way to play sound files.
//
// __Defense Sample for Game Programming Algorithms and Techniques
// Copyright (C) Sanjay Madhav. All rights reserved.
//
// Released under the Microsoft Permissive License.
// See LICENSE.txt for full details.
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace itp380
{
	public class SoundManager : Patterns.Singleton<SoundManager>
	{
		Dictionary<string, SoundEffect> m_Sounds = new Dictionary<string, SoundEffect>();
        Dictionary<string, Song> m_backgrounds = new Dictionary<string, Song>();

		public SoundManager()
		{

		}

		// Load the SFX
		public void LoadContent(ContentManager Content)
		{
			// Sounds
            m_Sounds.Add("MenuClick", Content.Load<SoundEffect>("Sounds/click"));
			m_Sounds.Add("Build", Content.Load<SoundEffect>("Sounds/Build"));
			m_Sounds.Add("GameOver", Content.Load<SoundEffect>("Sounds/GameOver"));
			m_Sounds.Add("Victory", Content.Load<SoundEffect>("Sounds/Victory"));
			m_Sounds.Add("Error", Content.Load<SoundEffect>("Sounds/Error"));
			m_Sounds.Add("Snared", Content.Load<SoundEffect>("Sounds/Snared"));
			m_Sounds.Add("Alarm", Content.Load<SoundEffect>("Sounds/Alarm"));
            m_Sounds.Add("Sword", Content.Load<SoundEffect>("Sounds/sword"));
            m_Sounds.Add("Shield", Content.Load<SoundEffect>("Sounds/shield"));
            m_Sounds.Add("Hit", Content.Load<SoundEffect>("Sounds/hit"));
            m_Sounds.Add("Heal", Content.Load<SoundEffect>("Sounds/heal"));
            m_Sounds.Add("Arrow", Content.Load<SoundEffect>("Sounds/arrow"));
            m_Sounds.Add("Potion", Content.Load<SoundEffect>("Sounds/potion"));
            
            // Background Music
            m_backgrounds.Add("Menu", Content.Load<Song>("Sounds/02 - Main Theme"));
            m_backgrounds.Add("Gameplay", Content.Load<Song>("Sounds/27 - The Battle Must Be Won"));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.08f;
             
		}

		public void PlaySoundCue(string cue)
		{
			m_Sounds[cue].Play();
		}

        public void PlayBackgroundMusic(string song)
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(m_backgrounds[song]);
        }
	}
}
