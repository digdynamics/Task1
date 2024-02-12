using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Task_1.Data;
using Task_1.Models;

namespace Task_1.Services
{
    public class CountryService
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public List<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountryById(int id)
        {
            return _context.Countries.Find(id);
        }

        public void AddCountry(Country country)
        {
            _context.Countries.Add(country);
            _context.SaveChanges();
        }

        public void UpdateCountry(Country country)
        {
            _context.Entry(country).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCountry(int id)
        {
            Country country = _context.Countries.Find(id);
            _context.Countries.Remove(country);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}