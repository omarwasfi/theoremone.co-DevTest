using System;
using System.Collections.Generic;
using System.Linq;

namespace FilterAndRank
{
    public static class Ranking
    {        
        public static IList<RankedResult> FilterByCountryWithRank(
            IList<Person> people,
            IList<CountryRanking> rankingData,
            IList<string> countryFilter,
            int minRank, int maxRank,
            int maxCount)
        {
            // TODO: write your solution here.  Do not change the method signature in any way, or validation of your solution will fail.


            rankingData =  filterThePeopleByCountryFilter(rankingData, countryFilter , minRank , maxRank,maxCount);

            people = removeThePeopleThatDontHaveCountryRank(people, rankingData);


            return people
                .Select(p => new RankedResult(p.Id, rankingData.First(r => r.PersonId == p.Id).Rank))
                .ToList();
        }

        private static IList<CountryRanking> filterThePeopleByCountryFilter(IList<CountryRanking> rankingData, IList<string> countryFilter,int minRank, int maxRank,int maxCount)
        {
            List<CountryRanking> removedCountryRanking = new List<CountryRanking>();

            foreach (CountryRanking countryRanking in rankingData)
            {
                bool isExist = countryFilter.Any(x => x == countryRanking.Country);
                if (isExist && countryRanking.Rank >= minRank  && countryRanking.Rank <=maxRank )
                {

                }
                else
                {
                    removedCountryRanking.Add(countryRanking);

                }

            }

            foreach (CountryRanking removedCountry  in removedCountryRanking)
            {
                rankingData.Remove(removedCountry);
            }
            List<CountryRanking> sortedCountryRanking = new List<CountryRanking>();

            foreach (string Key in countryFilter)
            {
                sortedCountryRanking.AddRange((from CountryRanking in rankingData
                                               orderby CountryRanking.Rank , CountryRanking.Country 
                                               where CountryRanking.Country == Key
                                     select CountryRanking).ToList());

            }
            sortedCountryRanking = sortedCountryRanking.OrderBy(x => x.Rank).ToList();

            int indexOfTheLastCount = maxCount - 1;
            int valueOfTheLastRank = sortedCountryRanking[indexOfTheLastCount].Rank;
            int valueOfTheNextLastRank = sortedCountryRanking[indexOfTheLastCount + 1].Rank;

           
            while (valueOfTheLastRank == valueOfTheNextLastRank)
            {
                maxCount = ++maxCount;
                indexOfTheLastCount = maxCount - 1;
                valueOfTheLastRank = sortedCountryRanking[indexOfTheLastCount].Rank;
                valueOfTheNextLastRank = sortedCountryRanking[indexOfTheLastCount + 1].Rank;
            }

            List<CountryRanking> finalResult = new List<CountryRanking>();

            for(int i = 0; i < maxCount; i++ )
            {
                finalResult.Add(sortedCountryRanking[i]);
            }

            return finalResult;
        }

        private static IList<Person> removeThePeopleThatDontHaveCountryRank( IList<Person> people , IList<CountryRanking> rankingData)
        {
            // Write a test 
            List<Person> removedPeople = new List<Person>();
            foreach(Person person in people)
            {

                bool isExist = rankingData.Any(r => r.PersonId == person.Id);

                if(!isExist)
                {
                    removedPeople.Add(person);
                }
            }

            foreach(Person removedPerson in removedPeople)
            {
                people.Remove(removedPerson);
            }


           

            return people;
        }
    }
}
