namespace csharp
{  

    public class Item
    {
        #region AgedBrie 
        interface IsAgedBrie
        {
            void handleExpired();
            void updateQuality();
            void initializeBackstagePassesAndSoForUs();

        }
        public class AgedBrie : IsAgedBrie
        {
            private Item itm;
            public AgedBrie(Item _itm)
            {
                itm = _itm;
            }
            public void handleExpired()
            {
                itm.increaseQualityIfNotMax();
            }
            public void updateQuality()
            {
                itm.increaseQualityIncludingBackstagePasses();
            }
            
            public void initializeBackstagePassesAndSoForUs()
            {
                itm.isBackstagePasses = new NotBackstagePasses(itm) as IsBackstagePasses;

                itm.isSulfuras = new NotSulfuras(itm) as IsSulfuras;
            }
        }
        public class NotAgedBrie : IsAgedBrie
        {
            private Item itm;
            public NotAgedBrie(Item _itm)
            {
                itm = _itm;
            }
            public void handleExpired()
            {
                itm.isBackstagePasses.handleExpired();
            }
            public void updateQuality()
            {
                itm.isBackstagePasses.updateQuality();
            }

            public void initializeBackstagePassesAndSoForUs()
            {
                itm.isBackstagePasses = itm.Name.Equals(BACKSTAGE_PASSES) ? new BackstagePasses(itm) as IsBackstagePasses
                                                                          : new NotBackstagePasses(itm) as IsBackstagePasses;
                itm.isBackstagePasses.initializeSulfuras();
            }
        }

        #endregion

        #region BackstagePasses 
        interface IsBackstagePasses
        {
            void initializeSulfuras();
            void handleExpired();

            void updateQuality();

            void increaseQuality();
        }
        class BackstagePasses : IsBackstagePasses
        {
            private Item itm;
            public BackstagePasses(Item _itm)
            {
                itm = _itm;
            }            
            public void handleExpired()
            {
                itm.Quality = 0;
            }

            public void updateQuality()
            {
                itm.increaseQualityIncludingBackstagePasses();
            }
            public void increaseQuality()
            {
                itm.increaseQualityIfFarFromExpiry();
                itm.increaseQualityIfCloseToExpiry();
            }            
            public void initializeSulfuras()
            {
                itm.isSulfuras = new NotSulfuras(itm) as IsSulfuras;
            }
        }
        class NotBackstagePasses : IsBackstagePasses
        {
            private Item itm;
            public NotBackstagePasses(Item _itm)
            {
                itm = _itm;
            }
            public void handleExpired()
            {
                itm.decreaseQualityIfItemHasQuality();
            }
            public void updateQuality()
            {
                itm.decreaseQualityIfItemHasQuality();
            }
            public void increaseQuality()
            {}

            public void initializeSulfuras()
            {
                itm.isSulfuras = itm.Name.Equals(SULFURAS) ? new Sulfuras(itm) as IsSulfuras
                                                               : new NotSulfuras(itm) as IsSulfuras;
            }
        }

        #endregion

        interface IsSulfuras
        {
            void decreaseQuality();
        }

        class Sulfuras : IsSulfuras
        {
            private Item itm;

            public Sulfuras(Item _itm)
            {
                itm = _itm;
            }
            public void decreaseQuality()
            {}
        }

        class NotSulfuras : IsSulfuras
        {
            private Item itm;

            public NotSulfuras(Item _itm)
            {
                itm = _itm;
            }
            
            public void decreaseQuality()
            {
                itm.Quality--;
            }
        }

        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }

        private IsAgedBrie isAgedBrie;

        private IsBackstagePasses isBackstagePasses;

        private IsSulfuras isSulfuras;

        private const string AGED_BRIE = "Aged Brie";

        private const string BACKSTAGE_PASSES = "Backstage passes to a TAFKAL80ETC concert";

        private const string SULFURAS = "Sulfuras, Hand of Ragnaros";

        private const int MAX_QUALITY = 50;

        private const int FAR_FROM_EXPIRY = 11;

        private const int CLOSED_TO_EXPIRY = 6;

        public override string ToString()
        {
            return this.Name + ", " + this.SellIn + ", " + this.Quality;
        }

        public Item()
        {
            isAgedBrie = Name.Equals(AGED_BRIE) ? new AgedBrie(this) as IsAgedBrie :
                                                  new NotAgedBrie(this) as IsAgedBrie;
            isAgedBrie.initializeBackstagePassesAndSoForUs();
        }

        public void increaseQualityIfNotMax()
        {
            if (this.Quality < MAX_QUALITY)
            {
                this.Quality++;
            }
        }

        public void increaseQualityIfFarFromExpiry()
        {
            if (this.SellIn < FAR_FROM_EXPIRY)
            {
                this.increaseQualityIfNotMax();
            }
        }

        public void increaseQualityIfCloseToExpiry()
        {
            if (this.SellIn < CLOSED_TO_EXPIRY)
            {
                this.increaseQualityIfNotMax();
            }
        }

        public void increaseQualityIncludingBackstagePasses()
        {
            if (this.Quality < MAX_QUALITY)
            {
                this.Quality++;
                isBackstagePasses.increaseQuality();
            }
        }

        public void decreaseQuality()
        {
            isSulfuras.decreaseQuality();
        }

        public void decreaseQualityIfItemHasQuality()
        {
            if (this.Quality > 0)
            {
                this.decreaseQuality();
            }
        }
        public void handleIfExpired()
        {
            if (this.SellIn < 0)
            {
                isAgedBrie.handleExpired();
            }
        }

        private void updateSellIn()
        {
            this.decreaseQuality();
            this.handleIfExpired();
        }

        public void update()
        {
            isAgedBrie.updateQuality();
            this.updateSellIn();
        }
    }
}
