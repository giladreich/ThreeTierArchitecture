﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamManager.Database;
using TeamManager.Main.ConceptTypes;
using TeamManager.Models.TechnicalConcept;

namespace TeamManager.Presenters
{
    /// <summary>
    /// The base presenter will contain the Concept Type that will be used in the way 
    /// we retrieve data from the database and all commmon data between all presenters.
    /// </summary>
    public abstract class BasePresenter
    {
        protected static ITechnicalConcept concept;


        public static void SetConceptAndDatabaseType(ConceptType conceptType, DatabaseType dbType)
        {
            switch (conceptType)
            {
                case ConceptType.First:
                    concept = new TechnicalConcept1(dbType);
                    break;

                case ConceptType.Second:
                    concept = new TechnicalConcept2(dbType);
                    break;

                default:
                    concept = new TechnicalConcept1(dbType);
                    break;
            }
        }
    }
}
