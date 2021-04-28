namespace csharp
{  

    public class Item
    {
        interface IsAgedBrie
        {
            bool isYes();
            void handleExpired();
            bool notBrieAndNotBackstagePasses();
            void updateQuality();
        }
        public class AgedBrie : IsAgedBrie
        {
            private Item itm;
            public AgedBrie(Item _itm)
            {
                itm = _itm;
            }
            public bool isYes()
            {
                return true;
            }
            public void handleExpired()
            {
                itm.increaseQualityIfNotMax();
            }

            public bool notBrieAndNotBackstagePasses()
            {
                return false;
            }
            public void updateQuality()
            {
                itm.increaseQualityIncludingBackstagePasses();
            }
        }
        public class NotAgedBrie : IsAgedBrie
        {
            private Item itm;
            public NotAgedBrie(Item _itm)
            {
                itm = _itm;
            }
            public bool isYes()
            {
                return false;
            }
            public void handleExpired()
            {
                itm.handleExpiredItemNotAgedBrie();
            }

            public bool notBrieAndNotBackstagePasses()
            {
                return !(itm.isBackstagePasses.isYes());
            }
            public void updateQuality()
            {
                itm.isBackstagePasses.updateQuality();
            }
        }     
        
        interface IsBackstagePasses
        {
            bool isYes();
            void handleExpiredItemNotAgedBrie();

            void updateQuality();
        }

        class BackstagePasses : IsBackstagePasses
        {
            private Item itm;
            public BackstagePasses(Item _itm)
            {
                itm = _itm;
            }
            public bool isYes()
            {
                return true;
            }

            public void handleExpiredItemNotAgedBrie()
            {
                itm.Quality = 0;
            }

            public void updateQuality()
            {
                itm.increaseQualityIncludingBackstagePasses();
            }
        }

        class NotBackstagePasses : IsBackstagePasses
        {
            private Item itm;

            public NotBackstagePasses(Item _itm)
            {
                itm = _itm;
            }
            public bool isYes()
            {
                return false;
            }

            public void handleExpiredItemNotAgedBrie()
            {
                itm.decreaseQualityIfItemHasQuality();
            }

            public void updateQuality()
            {
                itm.decreaseQualityIfItemHasQuality();
            }
        }


        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }

        private IsAgedBrie isAgedBrie;

        private IsBackstagePasses isBackstagePasses;

        private bool isSulfuras = false;

        private const string AGED_BRIE = "Aged Brie";

        private const string BACKSTAGE_PASSES_TO_A_TAFKAL80ETC_CONCERT = "Backstage passes to a TAFKAL80ETC concert";

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

            isBackstagePasses = this.Name.Equals(BACKSTAGE_PASSES_TO_A_TAFKAL80ETC_CONCERT) ?
                                                  new BackstagePasses(this) as IsBackstagePasses :
                                                  new NotBackstagePasses(this) as IsBackstagePasses;

            isSulfuras = this.Name.Equals(SULFURAS);
        }

        public void increaseQuality()
        {
            if (this.Quality < MAX_QUALITY)
                this.Quality++;
        }

        public void increaseQualityIfNotMax()
        {
            if (this.Quality < MAX_QUALITY)
            {
                this.increaseQuality();
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

        public void increaseQualityOfBackstagePasses()
        {
            if (isBackstagePasses.isYes())
            {
                this.increaseQualityIfFarFromExpiry();
                this.increaseQualityIfCloseToExpiry();
            }
        }

        public void increaseQualityIncludingBackstagePasses()
        {
            if (this.Quality < MAX_QUALITY)
            {
                this.increaseQuality();
                this.increaseQualityOfBackstagePasses();
            }
        }

        public void decreaseQuality()
        {
            if (!isSulfuras)
            {
                this.Quality--;
            }
        }

        public void decreaseQualityIfItemHasQuality()
        {
            if (this.Quality > 0)
            {
                this.decreaseQuality();
            }
        }

        private void updateQuality()
        {
            isAgedBrie.updateQuality();
        }

        private bool notBrieAndNotBackstagePasses()
        {
            return isAgedBrie.notBrieAndNotBackstagePasses();
        }

        public void handleExpiredItemNotAgedBrie()
        {
            isBackstagePasses.handleExpiredItemNotAgedBrie();
        }

        public void handleExpired()
        {
            isAgedBrie.handleExpired();
        }

        public void handleIfExpired()
        {
            if (this.SellIn < 0)
            {
                isAgedBrie.handleExpired();
            }
        }

        public void updateSellIn()
        {
            if (!isSulfuras)
            {
                this.SellIn--;
            }

            this.handleIfExpired();
        }

        public void update()
        {
            this.updateQuality();
            this.updateSellIn();
        }
    }
}
