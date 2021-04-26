using System.Collections.Generic;

namespace csharp
{
    public class GildedRose
    {
        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                updateQuality(Items[i]);
                updateSellIn(Items[i]);
            }
        }

        private void updateQuality(Item item)
        {
            if (item.Name != "Aged Brie" && item.Name != "Backstage passes to a TAFKAL80ETC concert")
            {
                decreaseQualityIfItemHasQuality(item);
            }
            else
            {
                increaseQualityIncludingBackstagePasses(item);
            }
        }

        private static void decreaseQualityIfItemHasQuality(Item item)
        {
            if (item.Quality > 0)
            {
                decreaseQuality(item);
            }
        }

        private static void decreaseQuality(Item item)
        {
            if (item.Name != "Sulfuras, Hand of Ragnaros")
            {
                item.Quality = item.Quality - 1;
            }
        }
        private static void increaseQualityIncludingBackstagePasses(Item item)
        {
            if (item.Quality < 50)
            {
                increaseQuality(item);
                increaseQualityOfBackstagePasses(item);
            }
        }        

        private static void increaseQualityOfBackstagePasses(Item item)
        {
            if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
            {
                increaseQualityIffarFromExpiry(item);
                increaseQualityIfCloseToExpiry(item);
            }
        }

        private static void increaseQualityIfCloseToExpiry(Item item)
        {
            if (item.SellIn < 6)
            {
                IncreaseQualityIfNotMax(item);
            }
        }

        private static void increaseQualityIffarFromExpiry(Item item)
        {
            if (item.SellIn < 11)
            {
                IncreaseQualityIfNotMax(item);
            }
        }

        private static void IncreaseQualityIfNotMax(Item item)
        {
            if (item.Quality < 50)
            {
                increaseQuality(item);
            }
        }


        private static void increaseQuality(Item item)
        {
            item.Quality = item.Quality + 1;
        }

        private void updateSellIn(Item item)
        {
            if (item.Name != "Sulfuras, Hand of Ragnaros")
            {
                item.SellIn = item.SellIn - 1;
            }

            handleIfExpired(item);
        }

        private void handleIfExpired(Item item)
        {
            if (item.SellIn < 0)
            {
                handleExpired(item);
            }
        }

        private static void handleExpired(Item item)
        {
            if (item.Name != "Aged Brie")
            {
                handleExpiredItemNotAgedBrie(item);
            }
            else
            {
                increaseQuality(item);
            }
        }

        private static void handleExpiredItemNotAgedBrie(Item item)
        {
            if (item.Name != "Backstage passes to a TAFKAL80ETC concert")
            {
                decreaseQualityIfItemHasQuality(item);
            }
            else
            {
                item.Quality = item.Quality - item.Quality;
            }
        }
    }
}
