﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
//Copyright (C) <2011 - 2013> <Jon Baker, Glenn Mariën and Lao Tszy>

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace fCraft {
    static class DevCommands {

        public static void Init () {
            /*
             * NOTE: These commands are unfinished, under development and non-supported.
             * If you are using a dev build of 800Craft, please comment these the below out to ensure 
             * stability.
             * */

            //CommandManager.RegisterCommand(CdFeed);
            //CommandManager.RegisterCommand(CdBot);
            //CommandManager.RegisterCommand(CdSpell);
            //CommandManager.RegisterCommand(CdGame);
        }

        static readonly CommandDescriptor CdGame = new CommandDescriptor {
            Name = "Game",
            Category = CommandCategory.World,
            Permissions = new Permission[] { Permission.Games },
            IsConsoleSafe = false,
            Usage = "/Unfinished command.",
            Handler = GameHandler
        };
        private static void GameHandler ( Player player, Command cmd ) {
            string GameMode = cmd.Next();
            string Option = cmd.Next();
            World world = player.World;
            /*if (world == WorldManager.MainWorld){
                player.Message("/Game cannot be used on the main world"); 
                return;
            }*/

            if ( GameMode.ToLower() == "zombie" ) {
                if ( Option.ToLower() == "start" ) {
                    ZombieGame game = new ZombieGame( player.World ); //move to world
                    game.Start();
                    return;
                } else {
                    CdGame.PrintUsage( player );
                    return;
                }
            }
            if ( GameMode.ToLower() == "minefield" ) {
                if ( Option.ToLower() == "start" ) {
                    if ( WorldManager.FindWorldExact( "Minefield" ) != null ) {
                        player.Message( "&WA game of Minefield is currently running and must first be stopped" );
                        return;
                    }
                    MineField.GetInstance();
                    MineField.Start( player );
                    return;
                } else if ( Option.ToLower() == "stop" ) {
                    if ( WorldManager.FindWorldExact( "Minefield" ) == null ) {
                        player.Message( "&WA game of Minefield is currently not running" );
                        return;
                    }
                    MineField.Stop( player, false );
                    return;
                } else {
                    CdGame.PrintUsage( player );
                    return;
                }
            } else {
                CdGame.PrintUsage( player );
                return;
            }
        }

        /*static readonly CommandDescriptor CdBot = new CommandDescriptor {
            Name = "Bot",
            Category = CommandCategory.Fun,
            Permissions = new[] { Permission.Chat },
            IsConsoleSafe = false,
            NotRepeatable = true,
            Usage = "/Spell",
            Help = "Penis",
            UsableByFrozenPlayers = false,
            Handler = BotHandler,
        };
        internal static void BotHandler ( Player player, Command cmd ) {
            Bot bot = player.Bot;
            string yes = cmd.Next();
            if ( yes.ToLower() == "create" ) {
                string Name = cmd.Next();
                Position Pos = new Position( player.Position.X, player.Position.Y, player.Position.Z, player.Position.R, player.Position.L );
                player.Bot = new Bot( Name, Pos, 1, player.World );
                //player.Bot.SetBot();
            }
        }*/

        static readonly CommandDescriptor CdSpell = new CommandDescriptor {
            Name = "Spell",
            Category = CommandCategory.Fun,
            Permissions = new[] { Permission.Chat },
            IsConsoleSafe = false,
            NotRepeatable = true,
            Usage = "/Spell",
            Help = "Penis",
            UsableByFrozenPlayers = false,
            Handler = SpellHandler,
        };
        public static SpellStartBehavior particleBehavior = new SpellStartBehavior();
        internal static void SpellHandler ( Player player, Command cmd ) {
            World world = player.World;
            Vector3I pos1 = player.Position.ToBlockCoords();
            Random _r = new Random();
            int n = _r.Next( 8, 12 );
            for ( int i = 0; i < n; ++i ) {
                double phi = -_r.NextDouble() + -player.Position.L * 2 * Math.PI;
                double ksi = -_r.NextDouble() + player.Position.R * Math.PI - Math.PI / 2.0;

                Vector3F direction = ( new Vector3F( ( float )( Math.Cos( phi ) * Math.Cos( ksi ) ), ( float )( Math.Sin( phi ) * Math.Cos( ksi ) ), ( float )Math.Sin( ksi ) ) ).Normalize();
                world.AddPhysicsTask( new Particle( world, ( pos1 + 2 * direction ).Round(), direction, player, Block.Obsidian, particleBehavior ), 0 );
            }
        }
    }
}
