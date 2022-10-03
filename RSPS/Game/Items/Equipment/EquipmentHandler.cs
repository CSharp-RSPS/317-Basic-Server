using RSPS.Data;
using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Combat;
using RSPS.Game.Items.Consumables;
using RSPS.Game.Items.Equipment.Definitions;
using RSPS.Game.Skills;
using RSPS.Game.UI;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment
{
    public class Legacy_AnimationDef
    {
        public int stand { get; set; }
        public int walk { get; set; }
        public int run { get; set; }
        public int hit { get; set; }
        public int block { get; set; }
        //public FightType fightType { get; set; }
        public int identity { get; set; }
    }
    public class Legacy_ItemBonus
    {
        public int attackStab { get; set; }
        public int attackSlash { get; set; }
        public int attackCrush { get; set; }
        public int attackRanged { get; set; }
        public int attackMagic { get; set; }
        public int defenceStab { get; set; }
        public int defenceSlash { get; set; }
        public int defenceCrush { get; set; }
        public int defenceRanged { get; set; }
        public int defenceMagic { get; set; }
        public int strength { get; set; }
        public int prayer { get; set; }
        public int speed { get; set; }
        public int identity { get; set; }
    }
    public class Legacy_ItemDistance
    {
        public int identity { get; set; }
        public int distance { get; set; }
    }
    public class Legacy_ItemEquip
    {
        public bool twoHanded { get; set; }
        public bool platebody { get; set; }
        public bool fullHelm { get; set; }
        public EquipType equipType { get; set; }
        public int identity { get; set; }
    }
    public class Legacy_ItemInfo
    {
        public string name { get; set; }
        public string description { get; set; }
        public int reverseIdentity { get; set; }
        public double weight { get; set; }
        public bool noted { get; set; }
        public bool stackable { get; set; }
        public bool member { get; set; }
        public bool shareable { get; set; }
        public bool enabled { get; set; }
        public int identity { get; set; }
    }
    public class Legacy_ItemInterface
    {
        public WeaponType type { get; set; }
        public int identity { get; set; }
    }
    public class Legacy_ItemPrizes
    {
        public int shopPrice { get; set; }
        public int highAlch { get; set; }
        public int lowAlch { get; set; }
        public int identity { get; set; }
    }
    public class Legacy_ItemReq
    {
        public int identity { get; set; }
        public Legacy_ItemSkillReq[] requirements { get; set; }
    }
    public class Legacy_ItemSkillReq
    {
        public int level { get; set; }
        public SkillType skillType { get; set; }
    }
    public class Legacy_ItemFood
    {
        public int identity { get; set; }
        public int heal { get; set; }
    }
    public class Legacy_BakeryFood
    {
        public int identity { get; set; }
        public int next_identity { get; set; }
        public int health { get; set; }
    }

    /// <summary>
    /// Handles equipment related operations
    /// </summary>
    public static class EquipmentHandler
    {

        /// <summary>
        /// The names of the possible equipment bonuses
        /// </summary>
        private static readonly string[] BonusNames = { "Stab", "Slash", "Crush",
            "Magic", "Range", "Stab", "Slash", "Crush", "Magic", "Range",
            "Strength", "Prayer" };

        /// <summary>
        /// Holds the available weapon interface definitions
        /// </summary>
        private static readonly Dictionary<WeaponType, WeaponInterfaceDef> WeaponInterfaceDefs = new();

        /// <summary>
        /// Holds the available equipment definitions
        /// </summary>
        private static readonly Dictionary<int, EquipDef> EquipDefs = new();

        /// <summary>
        /// Holds the available weapon definitions
        /// </summary>
        private static readonly Dictionary<int, WeaponDef> WeaponDefs = new();

        static EquipmentHandler()
        {
            JsonUtil.DataImport<WeaponInterfaceDef>("./Resources/items/weapon_interfaces.json", 
                wiDef => wiDef.ForEach(def => WeaponInterfaceDefs.Add(def.WeaponType, def)));

            JsonUtil.DataImport<EquipDef>("./Resources/items/equip_definitions.json", 
                eDef => eDef.ForEach(def => EquipDefs.Add(def.Id, def)));

            JsonUtil.DataImport<WeaponDef>("./Resources/items/weapon_definitions.json", 
                wDef => wDef.ForEach(def => WeaponDefs.Add(def.Id, def)));
        }

        public static void Temp() {
            Debug.WriteLine("alo");

            if (true)
                return;

            List<Legacy_AnimationDef> anims = JsonUtil.DeserializeListFromFile<Legacy_AnimationDef>("./Resources/items/item_animations.json");
            List<Legacy_ItemBonus> bonuses = JsonUtil.DeserializeListFromFile<Legacy_ItemBonus>("./Resources/items/item_bonuses.json");
            List<Legacy_ItemDistance> distances = JsonUtil.DeserializeListFromFile<Legacy_ItemDistance>("./Resources/items/item_distances.json");
            List<Legacy_ItemEquip> equips = JsonUtil.DeserializeListFromFile<Legacy_ItemEquip>("./Resources/items/item_equipping.json");
            List<Legacy_ItemInfo> infos = JsonUtil.DeserializeListFromFile<Legacy_ItemInfo>("./Resources/items/item_information.json");
            List<Legacy_ItemInterface> interfaces = JsonUtil.DeserializeListFromFile<Legacy_ItemInterface>("./Resources/items/item_interfaces.json");
            List<Legacy_ItemPrizes> prizes = JsonUtil.DeserializeListFromFile<Legacy_ItemPrizes>("./Resources/items/item_prizes.json");
            List<Legacy_ItemReq> reqs = JsonUtil.DeserializeListFromFile<Legacy_ItemReq>("./Resources/items/item_requirements.json");
            List<Legacy_ItemFood> foods = JsonUtil.DeserializeListFromFile<Legacy_ItemFood>("./Resources/items/food.json");
            List<Legacy_BakeryFood> bakeries = JsonUtil.DeserializeListFromFile<Legacy_BakeryFood>("./Resources/items/bakery_food.json");

            // Defs/Properties
            List<ItemDef> itemDefs = new();
            List<EquipDef> equipDefs = new();
            List<WeaponDef> wepDefs = new();

            infos.ForEach(i => {
                Legacy_ItemEquip equip = equips.FirstOrDefault(e => e.identity == i.identity);
                Legacy_AnimationDef anim = anims.FirstOrDefault(e => e.identity == i.identity);
                Legacy_ItemDistance distance = distances.FirstOrDefault(e => e.identity == i.identity);
                Legacy_ItemBonus bonus = bonuses.FirstOrDefault(e => e.identity == i.identity);
                Legacy_ItemInterface interfac = interfaces.FirstOrDefault(e => e.identity == i.identity);
                Legacy_ItemReq req = reqs.FirstOrDefault(e => e.identity == i.identity);
                Legacy_ItemFood? food = foods.FirstOrDefault(e => e.identity == i.identity);
                Legacy_BakeryFood? bakery = bakeries.FirstOrDefault(e => e.identity == i.identity);

                ItemDef def = new()
                {
                    Id = i.identity,
                    Name = i.name,
                    Description = i.description,
                    ReverseIdentity = i.reverseIdentity,
                    Weight = (short)i.weight,
                    Noted = i.noted,
                    Stackable = i.stackable,
                    Member = i.member,
                    Shareable = i.shareable,
                    Enabled = i.enabled,
                    Consumable = food != null || bakery != null,
                    EquipType = equip == null ? EquipType.None : equip.equipType
                };
                itemDefs.Add(def);

                if (def.Consumable)
                {
                    // TODO
                }
                if (equip != null)
                {
                    ItemCombatBonuses bonuses = new()
                    {
                        CrushAttackBonus = bonus == null ? 0 : bonus.attackCrush,
                        StabAttackBonus = bonus == null ? 0 : bonus.attackStab,
                        SlashAttackBonus = bonus == null ? 0 : bonus.attackSlash,
                        MagicAttackBonus = bonus == null ? 0 : bonus.attackMagic,
                        RangedAttackBonus = bonus == null ? 0 : bonus.attackRanged,
                        MagicDefenceBonus = bonus == null ? 0 : bonus.defenceMagic,
                        RangedDefenceBonus = bonus == null ? 0 : bonus.defenceRanged,
                        SlashDefenceBonus = bonus == null ? 0 : bonus.defenceSlash,
                        StabDefenceBonus = bonus == null ? 0 : bonus.defenceStab,
                        CrushDefenceBonus = bonus == null ? 0 : bonus.defenceCrush,
                        PrayerBonus = bonus == null ? 0 : bonus.prayer
                        
                    };
                    SkillRequirement[] skillReqs = Array.Empty<SkillRequirement>();

                    if (req != null && req.requirements != null)
                    {
                        skillReqs = new SkillRequirement[req.requirements.Length];

                        for (int ri = 0; ri < req.requirements.Length; ri++)
                        {
                            Legacy_ItemSkillReq lreq = req.requirements[ri];
                            skillReqs[ri] = new SkillRequirement()
                            {
                                SkillType = lreq.skillType,
                                Level = lreq.level,
                                BoostAllowed = false
                            };
                        }
                    }
                    EquipDef equipDef = new()
                    {
                        Id = def.Id,
                        EquipType = def.EquipType,
                        TwoHanded = equip.twoHanded,
                        Platebody = equip.platebody,
                        FullHelm = equip.fullHelm,
                        Bonuses = bonuses,
                        SkillRequirements = skillReqs
                    };
                    equipDefs.Add(equipDef);

                    if (equipDef.EquipType == EquipType.Weapon)
                    {
                        WeaponDef wepDef = new()
                        {
                            Id = equipDef.Id,
                            Speed = bonus == null ? 3000 : bonus.speed,
                            Distance = distance == null ? 1 : distance.distance,
                            WeaponType = interfac == null ? WeaponType.Unarmed : interfac.type,
                            Animations = new WeaponAnimations()
                            {
                                Stand = anim == null ? -1 : anim.stand,
                                Walk = anim == null ? -1 : anim.walk,
                                Run = anim == null ? -1 : anim.run,
                                Hit = anim == null ? -1 : anim.hit,
                                Block = anim == null ? -1 : anim.block,
                            }
                        };
                        wepDefs.Add(wepDef);
                    }
                }
            });
            //JsonUtil.SerializeListToFile("./Resources/export/weapon_interfaces.json", WeaponInterfaceDefs);
            //JsonUtil.SerializeListToFile("./Resources/export/item_definitions.json", itemDefs);
            JsonUtil.SerializeListToFile("./Resources/export/equip_definitions.json", equipDefs);
            //JsonUtil.SerializeListToFile("./Resources/export/weapon_definitions.json", wepDefs);*/

            Debug.WriteLine("Done");
        }

        /// <summary>
        /// Attempts to equip an item for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="itemId">The identifier of the item</param>
        /// <param name="slot">The item slot</param>
        public static void Equip(Player player, int itemId, int slot)
        {
            Item? inventoryItem = player.Inventory.GetItemBySlot(slot);

            if (inventoryItem == null || inventoryItem.Id != itemId)
            {
                return;
            }
            EquipDef? equipDef = GetEquipDef(itemId);

            if (equipDef == null || equipDef.EquipType == EquipType.None)
            { // Can not be equipped
                return;
            }
            if (equipDef.HasSkillRequirements
                && !SkillHandler.PassesSkillRequirements(player, equipDef.SkillRequirements, out SkillRequirement? unpassedSkillReq))
            { // Skill requirements not met
                PacketHandler.SendPacket(player, new SendMessage("You need a " + unpassedSkillReq?.SkillType.ToString()
                        + " level of at least " + unpassedSkillReq?.Level + " to wear this item."));
                return;
            }
            int equipmentSlot = equipDef.EquipType.GetAttributeOfType<EquipmentSlotAttribute>().Slot;
            Item? itemAtSlot = player.Equipment.GetItemBySlot(equipmentSlot);

            if (itemAtSlot != null)
            { // There is already an item in the equipment slot
                ItemDef? itemAtSlotDef = ItemManager.GetItemDefById(itemAtSlot.Id);
                
                if (itemAtSlotDef == null)
                {
                    return;
                }
                if (itemAtSlot.Id == inventoryItem.Id)
                {
                    if (!itemAtSlotDef.Stackable)
                    { // Swap the inventory and equipment item since it's the same ID
                        player.Equipment.Items[equipmentSlot] = inventoryItem;
                        player.Inventory.Items[slot] = itemAtSlot;
                        // Update the equipment, we don't need to take a possible weapon into account since it's the same item so the widget will not change
                        EquipmentUpdate(player);
                        return;
                    }
                    // Since the item is stackable, modify the amount of the current worn item
                    player.Equipment.ModifyItemAmount(itemAtSlot, equipmentSlot, inventoryItem.Amount);
                    // Remove the item (stack) from the inventory
                    player.Inventory.RemoveItemFromSlot(inventoryItem, slot);
                    // Update the equipment, use the current item in case of a weapon since only the quantity changed
                    EquipmentUpdate(player, equipDef.EquipType == EquipType.Weapon, itemAtSlot);
                    return;
                }
            }
            if (equipDef.TwoHanded)
            { // Make sure we can remove the shield if one is wielded
                int shieldSlot = EquipType.Shield.GetAttributeOfType<EquipmentSlotAttribute>().Slot;
                Item? shieldItem = player.Equipment.GetItemBySlot(shieldSlot);

                if (shieldItem != null)
                { // Is wearing an item in the shield slot
                    if (!player.Inventory.CanAddItem(shieldItem))
                    { // Unable to unequip item from shield slot
                        PacketHandler.SendPacket(player, new SendMessage("You don't have enough space in your inventory."));
                        return;
                    }
                    player.Equipment.RemoveItemFromSlot(shieldItem, shieldSlot);
                    player.Inventory.AddItem(shieldItem);
                }
            }
            if (itemAtSlot != null)
            { // Remove the equiped item from it's slot
                player.Equipment.RemoveItemFromSlot(itemAtSlot, equipmentSlot);
            }
            player.Inventory.RemoveItemFromSlot(inventoryItem, slot);
            player.Equipment.AddItem(inventoryItem, equipmentSlot);

            if (equipDef.EquipType == EquipType.Shield)
            { // We're equipping a shield
                int weaponSlot = EquipType.Weapon.GetAttributeOfType<EquipmentSlotAttribute>().Slot;
                Item? weaponItem = player.Equipment.GetItemBySlot(weaponSlot);

                if (weaponItem != null)
                {
                    EquipDef? wepDef = GetEquipDef(weaponItem.Id);

                    if (wepDef != null && wepDef.TwoHanded)
                    { // We're wearing a 2-handed item so we have to remove it in order to wield the shield
                        player.Equipment.RemoveItemFromSlot(weaponItem, weaponSlot);
                        player.Inventory.AddItem(weaponItem);
                        // Update the equipment indicating the weapon changed in order to update the combat widgets
                        EquipmentUpdate(player, true, weaponItem);
                        return;
                    }
                }
            }
            EquipmentUpdate(player, equipDef.EquipType == EquipType.Weapon, inventoryItem);
        }

        /// <summary>
        /// Attempts to un-equip an item for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="itemId">The identifier of the item</param>
        /// <param name="slot">The item slot</param>
        public static void Unequip(Player player, int itemId, int slot)
        {
            Item? item = player.Equipment.GetItemBySlot(slot);

            if (item == null || item.Id != itemId)
            {
                return;
            }
            if (!player.Inventory.CanAddItem(item))
            {
                PacketHandler.SendPacket(player, new SendMessage("You don't have enough space in your inventory."));
                return;
            }
            player.Equipment.RemoveItemFromSlot(item, slot);
            player.Inventory.AddItem(item);

            EquipmentUpdate(player, GetEquipType(itemId) == EquipType.Weapon, null);
        }

        /// <summary>
        /// Indicates equipment updated for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="weapon">Whether the weapon got updated</param>
        /// <param name="item">The updated weapon item</param>
        private static void EquipmentUpdate(Player player, bool weapon = false, Item? item = null)
        {
            ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.Inventory);
            ItemManager.RefreshInterfaceItems(player, player.Equipment.Items, Interfaces.Equipment);

            UpdateEquipmentBonuses(player);
            UpdateWeight(player);

            if (weapon)
            {
                WriteWeaponInterface(player, item);
            }
            player.AppearanceUpdateRequired = true;
            player.UpdateRequired = true;
        }

        /// <summary>
        /// Writes the weapon interface for an item
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="item">The item</param>
        private static void WriteWeaponInterface(Player player, Item? item)
        {
            WeaponType wiType = item == null ? WeaponType.Unarmed : GetWeaponType(item.Id);
            WeaponInterfaceDef? wiDef = GetWeaponInterfaceDef(wiType);

            if (wiDef == null)
            {
                throw new Exception("No weapon interface definition found for " + wiType.ToString());
            }
            ItemDef? itemDef = item == null ? null : ItemManager.GetItemDefById(item.Id);

            if (item != null && itemDef == null)
            {
                throw new Exception("No item definition found for " + item.Id);
            }
            if (wiType == WeaponType.Crossbow || wiType == WeaponType.Whip)
            { // Write "Weapon: " in front of where the weapon name comes for crossbows and whips
                PacketHandler.SendPacket(player, new SendSetInterfaceText(wiDef.NameLineId - 1, "Weapon: "));
            }
            // Write the weapon (or unarmed) text to the interface
            PacketHandler.SendPacket(player, new SendSetInterfaceText(wiDef.NameLineId, 
                " " + (wiType == WeaponType.Unarmed ? "Unarmed" : (itemDef == null ? "Undefined" : itemDef.Name))));
            // Update the combat style sidebar interface to match the wielded weapon
            PacketHandler.SendPacket(player, new SendSidebarInterface(0, wiDef.InterfaceId));
            // Make the special bar visible if the weapon has a special attack
            PacketHandler.SendPacket(player, new SendHiddenInterface(wiDef.SpecialBarId, wiDef.SpecialBarId > -1));

            if (item != null && itemDef != null)
            { // Display the weapon model onto the interface
                PacketHandler.SendPacket(player, new SendInterfaceItem(wiDef.InterfaceId + 1, 200, item.Id));
            }
            if (item != null && wiDef.SpecialBarId > -1)
            { // Update the special energy for the weapon if it has a special attack
                UpdateSpecialEnergy(player, item, wiDef);
            }
        }

        /// <summary>
        /// Updates the special energy for a weapon item
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="item">The item</param>
        /// <param name="wiDef">The weapon interface definition</param>
        private static void UpdateSpecialEnergy(Player player, Item item, WeaponInterfaceDef wiDef)
        {
            int meterId = wiDef.SpecialMeterId;
            int specEnergy = player.PersistentVars.SpecialEnergy;

            for (int i = 10; i > 0; i--)
            {
                PacketHandler.SendPacket(player, new SendInterfaceOffset(--meterId, specEnergy >= (i * 10) ? 500 : 0, 0));
            }
            PacketHandler.SendPacket(player, new SendSetInterfaceText(meterId, "@whi@Special  ATTACK"));
        }

        /// <summary>
        /// Updates the carried weight for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void UpdateWeight(Player player)
        {
            List<Item> items = new();

            player.Equipment.Items.Values.ToList().ForEach(item => { 
                if (item != null)
                {
                    items.Add(item);
                }
            });
            player.Inventory.Items.Values.ToList().ForEach(item => {
                if (item != null)
                {
                    items.Add(item);
                }
            });
            short weight = 0;

            foreach (Item item in items)
            {
                ItemDef? def = ItemManager.GetItemDefById(item.Id);

                if (def == null)
                {
                    continue;
                }
                weight += def.Weight;
            }
            PacketHandler.SendPacket(player, new SendWeight(weight));
        }

        /// <summary>
        /// Updates the bonuses of a player's equipment to their interface
        /// </summary>
        /// <param name="player">The player</param>
        public static void UpdateEquipmentBonuses(Player player)
        {
            int[] bonuses = new int[12];

            for (int i = 0; i < player.Equipment.Capacity; i++)
            {
                Item? item = player.Equipment.GetItemBySlot(i);

                if (item == null)
                {
                    continue;
                }
                EquipDef? def = GetEquipDef(item.Id);

                if (def == null)
                {
                    continue;
                }
                bonuses[0] += def.Bonuses.StabAttackBonus;
                bonuses[1] += def.Bonuses.SlashAttackBonus;
                bonuses[2] += def.Bonuses.CrushAttackBonus;
                bonuses[3] += def.Bonuses.RangedAttackBonus;
                bonuses[4] += def.Bonuses.MagicAttackBonus;
                bonuses[5] += def.Bonuses.StabDefenceBonus;
                bonuses[6] += def.Bonuses.SlashDefenceBonus;
                bonuses[7] += def.Bonuses.CrushDefenceBonus;
                bonuses[8] += def.Bonuses.RangedDefenceBonus;
                bonuses[9] += def.Bonuses.MagicDefenceBonus;
                bonuses[10] += def.Bonuses.StrengthBonus;
                bonuses[11] += def.Bonuses.PrayerBonus;
            }
            for (int i = 0; i < bonuses.Length; i++)
            {
                int bonus = bonuses[i];

                int interfaceId = 1675 + i + (i == 10 ? 1 : 0);
                string interfaceText = BonusNames[i] + ": " + (bonus >= 0 ? "+" : "-") + Math.Abs(bonus);

                PacketHandler.SendPacket(player, new SendSetInterfaceText(interfaceId, interfaceText));
            } //TODO: prayer bonus always seems 120
        }

        /// <summary>
        /// Retrieves the combat bonus for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="skillType">The skill type</param>
        /// <param name="fightStyle">the fighting style</param>
        /// <param name="offensive">Whether we need offensive or defensive stats</param>
        /// <returns>The bonus</returns>
        public static int GetCombatBonus(Player player, SkillType skillType, FightStyle fightStyle, bool offensive)
        {
            if (skillType != SkillType.Attack && skillType != SkillType.Strength
                && skillType != SkillType.Defence && skillType != SkillType.Ranged
                && skillType != SkillType.Magic)
            {
                throw new Exception("Invalid skill type " + skillType.ToString() + " for calculating combat bonus");
            }
            int bonus = 0;

            foreach (Item? item in player.Equipment.Items.Values)
            {
                if (item == null)
                {
                    continue;
                }
                EquipDef? def = GetEquipDef(item.Id);

                if (def == null)
                {
                    continue;
                }
                switch (skillType)
                {
                    case SkillType.Strength:
                        bonus += offensive ? def.Bonuses.StrengthBonus : 0;
                        break;

                    case SkillType.Ranged:
                        bonus += offensive ? def.Bonuses.RangedAttackBonus : def.Bonuses.RangedDefenceBonus;
                        break;

                    case SkillType.Magic:
                        bonus += offensive ? def.Bonuses.MagicAttackBonus : def.Bonuses.MagicDefenceBonus;
                        break;
                }
                if (skillType == SkillType.Attack || skillType == SkillType.Defence)
                {
                    if ((skillType == SkillType.Attack && !offensive)
                        || (skillType == SkillType.Defence && offensive))
                    {
                        continue;
                    }
                    switch (fightStyle)
                    {
                        case FightStyle.Stab:
                            bonus += offensive ? def.Bonuses.StabAttackBonus : def.Bonuses.StabDefenceBonus;
                            break;

                        case FightStyle.Slash:
                            bonus += offensive ? def.Bonuses.SlashAttackBonus : def.Bonuses.SlashDefenceBonus;
                            break;

                        case FightStyle.Crush:
                            bonus += offensive ? def.Bonuses.CrushAttackBonus : def.Bonuses.CrushDefenceBonus;
                            break;
                    }
                }
            }
            return bonus;
        }

        /// <summary>
        /// Retrieves the weapon interface definition for a given weapont type
        /// </summary>
        /// <param name="weaponType">The weapon type</param>
        /// <returns>The weapon interface definition</returns>
        private static WeaponInterfaceDef? GetWeaponInterfaceDef(WeaponType weaponType)
        {
            return WeaponInterfaceDefs.ContainsKey(weaponType) ? WeaponInterfaceDefs[weaponType] : null;
        }

        /// <summary>
        /// Retrieves the type of weapon for a given item identifier
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The weapon type</returns>
        private static WeaponType GetWeaponType(int itemId)
        {
            WeaponDef? def = GetWeaponDef(itemId);
            return def == null ? WeaponType.Unarmed : def.WeaponType;
        }

        /// <summary>
        /// Retrieves the weapon definition for a given item identifier
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The weapon definition</returns>
        private static WeaponDef? GetWeaponDef(int itemId)
        {
            return WeaponDefs.ContainsKey(itemId) ? WeaponDefs[itemId] : null;
        }

        /// <summary>
        /// Retrieves the equip type for a given item identifier
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The equip type</returns>
        private static EquipType GetEquipType(int itemId)
        {
            EquipDef? def = GetEquipDef(itemId);
            return def == null ? EquipType.None : def.EquipType;
        }

        /// <summary>
        /// Retrieves the equip definition for a given item identifier
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The equip definition</returns>
        private static EquipDef? GetEquipDef(int itemId)
        {
            return EquipDefs.ContainsKey(itemId) ? EquipDefs[itemId] : null;
        }

    }
}
