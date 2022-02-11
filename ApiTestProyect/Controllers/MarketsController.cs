using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTestProyect.Models;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using PagedList.Core;

namespace ApiTestProyect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketsController : ControllerBase
    {
        private readonly MarketContext _context;

        public MarketsController(MarketContext context)
        {
            _context = context;
        }

        // GET: api/Markets
        [HttpGet]
        public ActionResult<IEnumerable<Market>> GetMarkets(string search, int? page,int? pagesize, string filtercountry, int? filteripoYear)
        {
            var markets = from x in _context.Markets 
                          select x;
            int pageSize = pagesize ?? 3;
            int pageNumber = page ?? 1;
            if (search is not null)
            {
                return  markets.Where(x => x.country.Contains(search) || x.industry.Contains(search) || x.name.Contains(search) || x.sector.Contains(search) || x.symbol.Contains(search)).ToList();
            }
            if(!string.IsNullOrEmpty(filtercountry) && filteripoYear is null)
            {
                return markets.Where(x => x.country.Equals(filtercountry)).ToList();
            }
            else if(!string.IsNullOrEmpty(filtercountry) && filteripoYear is not null)
            {
                return markets.Where(x => x.ipoYear==filteripoYear).ToList();
            }
            else if(string.IsNullOrEmpty(filtercountry) && filteripoYear is not null)
            {
                return markets.Where(x => x.ipoYear == filteripoYear && x.country.Equals(filtercountry)).ToList();
            }            
            var marketspag = markets.ToPagedList(pageNumber, pageSize);
            return Ok(marketspag);
        }

        // GET: api/Markets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Market>> GetMarket(int id)
        {
            var market = await _context.Markets.FindAsync(id);

            if (market == null)
            {
                return NotFound();
            }

            return market;
        }

        // PUT: api/Markets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarket(int id, Market market)
        {
            if (id != market.id)
            {
                return BadRequest();
            }

            _context.Entry(market).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Markets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Market>> PostMarket(Market market)
        {
            market.createdAt = DateTime.Now;
            market.updatedAt = DateTime.Now;
            _context.Markets.Add(market);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMarket", new { id = market.id }, market);
        }

        // DELETE: api/Markets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarket(int id)
        {
            var market = await _context.Markets.FindAsync(id);
            if (market == null)
            {
                return NotFound();
            }

            _context.Markets.Remove(market);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Export to pdf a range market
        /// </summary>
        /// <param name="firtsmarket"></param>
        /// <param name="lastmarket"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/ExportPdf")]
        public async Task<ActionResult<string>> ExportPdf(int firtsmarket, int lastmarket, string Route)
        {

            var market = await _context.Markets.ToListAsync();
            var marketrange = market.FindAll(x => x.id >= firtsmarket && x.id <= lastmarket);

            Document doc = new Document(PageSize.LETTER);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@Route+"\\Market.pdf", FileMode.Create));
            doc.AddTitle("Markets from " + firtsmarket + " to " + lastmarket);
            doc.AddCreator("Usertest");
            doc.Open();
            Font _standardFont = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK);
            doc.Add(new Paragraph("Markets from " + firtsmarket + " to " + lastmarket));
            doc.Add(Chunk.NEWLINE);
            PdfPTable tblMarkets = new PdfPTable(13);
            tblMarkets.WidthPercentage = 100;

            PdfPCell symbol = new PdfPCell(new Phrase("Symbol", _standardFont));
            symbol.BorderWidth = 0;
            symbol.BorderWidthBottom = 0.75f;

            PdfPCell name = new PdfPCell(new Phrase("Name", _standardFont));
            name.BorderWidth = 0;
            name.BorderWidthBottom = 0.75f;

            PdfPCell country = new PdfPCell(new Phrase("Country", _standardFont));
            country.BorderWidth = 0;
            country.BorderWidthBottom = 0.75f;

            PdfPCell industry = new PdfPCell(new Phrase("Industry", _standardFont));
            industry.BorderWidth = 0;
            industry.BorderWidthBottom = 0.75f;

            PdfPCell ipoYear = new PdfPCell(new Phrase("IpoYear", _standardFont));
            ipoYear.BorderWidth = 0;
            ipoYear.BorderWidthBottom = 0.75f;

            PdfPCell marketCap = new PdfPCell(new Phrase("MarketCap", _standardFont));
            marketCap.BorderWidth = 0;
            marketCap.BorderWidthBottom = 0.75f;

            PdfPCell sector = new PdfPCell(new Phrase("Sector", _standardFont));
            sector.BorderWidth = 0;
            sector.BorderWidthBottom = 0.75f;

            PdfPCell volume = new PdfPCell(new Phrase("Volume", _standardFont));
            volume.BorderWidth = 0;
            volume.BorderWidthBottom = 0.75f;

            PdfPCell netChange = new PdfPCell(new Phrase("NetChange", _standardFont));
            netChange.BorderWidth = 0;
            netChange.BorderWidthBottom = 0.75f;

            PdfPCell netChangePercent = new PdfPCell(new Phrase("NetChangePercent", _standardFont));
            netChangePercent.BorderWidth = 0;
            netChangePercent.BorderWidthBottom = 0.75f;

            PdfPCell lastPrice = new PdfPCell(new Phrase("LastPrice", _standardFont));
            lastPrice.BorderWidth = 0;
            lastPrice.BorderWidthBottom = 0.75f;


            PdfPCell createdAt = new PdfPCell(new Phrase("CreatedAt", _standardFont));
            createdAt.BorderWidth = 0;
            createdAt.BorderWidthBottom = 0.75f;


            PdfPCell updatedAt = new PdfPCell(new Phrase("UpdatedAt", _standardFont));
            updatedAt.BorderWidth = 0;
            updatedAt.BorderWidthBottom = 0.75f;

            tblMarkets.AddCell(symbol);
            tblMarkets.AddCell(name);
            tblMarkets.AddCell(country);
            tblMarkets.AddCell(industry);
            tblMarkets.AddCell(ipoYear);
            tblMarkets.AddCell(marketCap);
            tblMarkets.AddCell(sector);
            tblMarkets.AddCell(volume);
            tblMarkets.AddCell(netChange);
            tblMarkets.AddCell(netChangePercent);
            tblMarkets.AddCell(lastPrice);
            tblMarkets.AddCell(createdAt);
            tblMarkets.AddCell(updatedAt);

            foreach (var item in marketrange)
            {
                symbol = new PdfPCell(new Phrase(item.symbol, _standardFont));
                symbol.BorderWidth = 0;

                name = new PdfPCell(new Phrase(item.name, _standardFont));
                name.BorderWidth = 0;

                country = new PdfPCell(new Phrase(item.country, _standardFont));
                country.BorderWidth = 0;

                industry = new PdfPCell(new Phrase(item.industry, _standardFont));
                industry.BorderWidth = 0;

                ipoYear = new PdfPCell(new Phrase(item.ipoYear.ToString(), _standardFont));
                ipoYear.BorderWidth = 0;

                marketCap = new PdfPCell(new Phrase(item.marketCap.ToString(), _standardFont));
                marketCap.BorderWidth = 0;

                sector = new PdfPCell(new Phrase(item.sector.ToString(), _standardFont));
                sector.BorderWidth = 0;

                volume = new PdfPCell(new Phrase(item.volume.ToString(), _standardFont));
                volume.BorderWidth = 0;

                netChange = new PdfPCell(new Phrase(item.netChange.ToString(), _standardFont));
                netChange.BorderWidth = 0;

                netChangePercent = new PdfPCell(new Phrase(item.netChangePercent.ToString(), _standardFont));
                netChangePercent.BorderWidth = 0;

                lastPrice = new PdfPCell(new Phrase(item.lastPrice.ToString(), _standardFont));
                lastPrice.BorderWidth = 0;


                createdAt = new PdfPCell(new Phrase(item.createdAt.ToString(), _standardFont));
                createdAt.BorderWidth = 0;


                updatedAt = new PdfPCell(new Phrase(item.updatedAt.ToString(), _standardFont));
                updatedAt.BorderWidth = 0;

                tblMarkets.AddCell(symbol);
                tblMarkets.AddCell(name);
                tblMarkets.AddCell(country);
                tblMarkets.AddCell(industry);
                tblMarkets.AddCell(ipoYear);
                tblMarkets.AddCell(marketCap);
                tblMarkets.AddCell(sector);
                tblMarkets.AddCell(volume);
                tblMarkets.AddCell(netChange);
                tblMarkets.AddCell(netChangePercent);
                tblMarkets.AddCell(lastPrice);
                tblMarkets.AddCell(createdAt);
                tblMarkets.AddCell(updatedAt);
            }
            doc.Add(tblMarkets);

            doc.Close();
            writer.Close();

            return $"Pdf download in {Route}\\Market.pdf";
        }

        private bool MarketExists(int id)
        {
            return _context.Markets.Any(e => e.id == id);
        }
    }
}
