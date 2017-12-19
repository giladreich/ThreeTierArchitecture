﻿using System.Collections.Generic;
using System.Linq;
using log4net;
using TeamManager.Models.ResourceData;
using TeamManager.Presenters.Events;
using TeamManager.Utilities;
using TeamManager.Views.Enums;
using TeamManager.Views.Interfaces;

namespace TeamManager.Presenters
{
    public class UnsignedPlayersPresenter : BasePresenter
    {
        /// <summary> Logger instance of the class <see cref="UnsignedPlayersPresenter"/> </summary>
        private static readonly ILog Log = Logger.GetLogger();

        private readonly IUnsignedPlayersView _view;



        public UnsignedPlayersPresenter(IUnsignedPlayersView view)
        {
            _view = view;
            view.PlayersListBox.Clear();

            ChildClosed += Window_ChildClose;
        }



        public void BindPlayersData()
        {
            Log.Info("Binding players data to listbox.");
            _view.PlayersListBox.Clear();
            List<Player> players = Concept.GetAllPlayers()?.Where(p => p.TeamId == "0").ToList();
            if (players.IsNullOrEmpty()) return;

            players?.ForEach(player => _view.PlayersListBox.Add(player));
            _view.PlayerSelectedIndex = 0;
        }

        public void DeletePlayer()
        {
            if (_view.PlayerSelectedIndex == -1) return;

            Log.Info("Deleting player.");
            int pSelIndex = _view.PlayerSelectedIndex;
            Player player = _view.PlayersListBox[pSelIndex].ToPlayer();
            if (player == null) return; 

            Concept.RemovePlayer(player.Id);
            _view.PlayersListBox.RemoveAt(pSelIndex);
            if (_view.PlayersListBox.Count == pSelIndex)
                pSelIndex--;

            _view.PlayerSelectedIndex = pSelIndex;
        }

        public Player GetPlayer()
        {
            int pSelIndex = _view.PlayerSelectedIndex;
            if (pSelIndex == -1) return null;

            return _view.PlayersListBox[pSelIndex].ToPlayer();
        }

        void Window_ChildClose(object sender, PresenterArgs args)
        {
            int pSelIndex = _view.PlayerSelectedIndex;
            BindPlayersData();

            if (_view.PlayersListBox.Count == pSelIndex)
                pSelIndex--;

            _view.PlayerSelectedIndex = pSelIndex;
        }

        public override void WindowClosed()
        {
            OnChildClosed(this, new PresenterArgs(WindowType.UnsignedPlayers));
        }

    }
}
