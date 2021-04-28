namespace csharp
{  
    public class Item
    {
        #region Item Interface
        interface Item_TMP
        {
            void handleExpired();
            void updateQuality();
            void increaseQuality();
            void decreaseQuality();
        }

        #endregion

        #region AgedBrie 

        public class AgedBrie : Item_TMP
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
            public void increaseQuality()
            {}
            public void decreaseQuality()
            {
                itm.Quality--;
            }
        }        

        #endregion

        #region BackstagePasses 
        
        class BackstagePasses :  Item_TMP
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
            public void decreaseQuality()
            {
                itm.Quality--;
            }
        }
        class NotBackstagePasses :  Item_TMP
        {
            private IsSulfuras isSulfuras;
            private Item itm;
            public NotBackstagePasses(Item _itm)
            {
                itm = _itm;
                isSulfuras = itm.Name.Equals(SULFURAS) ? new Sulfuras(itm) as IsSulfuras
                                                               : new NotSulfuras(itm) as IsSulfuras;
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
            public void decreaseQuality()
            {
                isSulfuras.decreaseQuality();
            }
        }

        #endregion

        #region Sulfuras
        interface IsSulfuras
        {
            void decreaseQuality();
            void handleExpired();
        }
        class Sulfuras : IsSulfuras
        {
            private Item itm;

            public Sulfuras(Item _itm)
            {
                itm = _itm;
            }
            public void decreaseQuality()
            { }

            public void handleExpired()
            {
                itm.decreaseQualityIfItemHasQuality();
            }
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

            public void handleExpired()
            {
                itm.decreaseQualityIfItemHasQuality();
            }
        }

        #endregion

        #region Attributes

        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }

        #endregion

        #region Constants/Variables

        private Item_TMP isAgedBrie;        

        private const string AGED_BRIE = "Aged Brie";

        private const string BACKSTAGE_PASSES = "Backstage passes to a TAFKAL80ETC concert";

        private const string SULFURAS = "Sulfuras, Hand of Ragnaros";

        private const int MAX_QUALITY = 50;

        private const int FAR_FROM_EXPIRY = 11;

        private const int CLOSED_TO_EXPIRY = 6;

        #endregion

        #region Methods
        public override string ToString()
        {
            return this.Name + ", " + this.SellIn + ", " + this.Quality;
        }
        private void SetItem()
        {
            isAgedBrie = Name.Equals(AGED_BRIE)
                ? new AgedBrie(this) as Item_TMP
                : this.Name.Equals(BACKSTAGE_PASSES)
                ? new BackstagePasses(this) as Item_TMP
                : new NotBackstagePasses(this) as Item_TMP;
        }
        private void increaseQualityIfNotMax()
        {
            if (this.Quality < MAX_QUALITY)
            {
                this.Quality++;
            }
        }
        private void increaseQualityIfFarFromExpiry()
        {
            if (this.SellIn < FAR_FROM_EXPIRY)
            {
                this.increaseQualityIfNotMax();
            }
        }
        private void increaseQualityIfCloseToExpiry()
        {
            if (this.SellIn < CLOSED_TO_EXPIRY)
            {
                this.increaseQualityIfNotMax();
            }
        }
        private void increaseQualityIncludingBackstagePasses()
        {
            if (this.Quality < MAX_QUALITY)
            {
                this.Quality++;
                isAgedBrie.increaseQuality();
            }
        }
        private void decreaseQualityIfItemHasQuality()
        {
            if (this.Quality > 0)
            {
                isAgedBrie.decreaseQuality();
            }
        }
        private void handleIfExpired()
        {
            if (this.SellIn < 0)
            {
                isAgedBrie.handleExpired();
            }
        }
        private void updateSellIn()
        {
            isAgedBrie.decreaseQuality();
            this.handleIfExpired();
        }
        public void update()
        {
            SetItem();
            isAgedBrie.updateQuality();
            this.updateSellIn();
        }

        #endregion
    }
}
