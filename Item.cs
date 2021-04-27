namespace csharp
{
    public class Item
    {
        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }

        public override string ToString()
        {
            return this.Name + ", " + this.SellIn + ", " + this.Quality;
        }

        public void increaseQuality()
        {
            this.Quality = this.Quality + 1;
        }

        public void increaseQualityIfNotMax()
        {
            if (this.Quality < 50)
            {
                this.increaseQuality();
            }
        }

        public void increaseQualityIfFarFromExpiry()
        {
            if (this.SellIn < 11)
            {
                this.increaseQualityIfNotMax();
            }
        }

        public void increaseQualityIfCloseToExpiry()
        {
            if (this.SellIn < 6)
            {
                this.increaseQualityIfNotMax();
            }
        }

        public void increaseQualityOfBackstagePasses()
        {
            if (!this.Name.Equals("Backstage passes to a TAFKAL80ETC concert"))
            {
                this.increaseQualityIfFarFromExpiry();
                this.increaseQualityIfCloseToExpiry();
            }
        }

        public void increaseQualityIncludingBackstagePasses()
        {
            if (this.Quality < 50)
            {
                this.increaseQuality();
                this.increaseQualityOfBackstagePasses();
            }
        }

        public void decreaseQuality()
        {
            if (!this.Name.Equals("Sulfuras, Hand of Ragnaros"))
            {
                this.Quality = this.Quality - 1;
            }
        }

        public void decreaseQualityIfItemHasQuality()
        {
            if (this.Quality > 0)
            {
                this.decreaseQuality();
            }
        }

        public void updateQuality()
        {
            if (!this.Name.Equals("Aged Brie") && !this.Name.Equals("Backstage passes to a TAFKAL80ETC concert"))
            {
                this.decreaseQualityIfItemHasQuality();
            }
            else
            {
                this.increaseQualityIncludingBackstagePasses();
            }
        }

        public void handleExpiredItemNotAgedBrie()
        {
            if (!this.Name.Equals("Backstage passes to a TAFKAL80ETC concert"))
            {
                this.decreaseQualityIfItemHasQuality();
            }
            else
            {
                this.Quality = this.Quality - this.Quality;
            }
        }

        public void handleExpired()
        {
            if (!this.Name.Equals("Aged Brie"))
            {
                this.handleExpiredItemNotAgedBrie();
            }
            else
            {
                this.increaseQualityIfNotMax();
            }
        }

        public void handleIfExpired()
        {
            if (this.SellIn < 0)
            {
                this.handleExpired();
            }
        }

        public void updateSellIn()
        {
            if (this.Name != "Sulfuras, Hand of Ragnaros")
            {
                this.SellIn = this.SellIn - 1;
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
