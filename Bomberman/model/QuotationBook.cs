﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.model
{
    public static class QuotationBook
    {
        private static string[] citations =
        {
            "Жизнь так жестока, почему жизнь за гробом должна быть иной?",
            "Слушай беззвучие, слушай и наслаждайся тем, чего тебе не давали в жизни, — тишиной.",
            "Интересно было бы поглядеть на то, что от меня останется, когда меня не останется.",
            "— Думаешь, мы действительно будем сидеть на облаке и говорить о море?\r\n— Да, я твёрдо в это верю.",
            "— Думаешь, мы все живем несколько раз?\r\n— Конечно.\r\n— Хреново...",
            "— Я умер?!!\r\n— Соболезную.",
            "Я был здесь всего мгновение.",
            "Быть красивыми\r\nПосле смерти умеют\r\nТолько деревья.",
            "Люди верят в жизнь после смерти потому, что без этой веры им совсем невыносимо.",
            "Воскресать могут только мертвые. Живым — труднее.",
            "Жизнь хорошая штука... А загробная жизнь лучше!",
            "Если б мир был морем с водкой\r\nЯ бы был подводной лодкой",
            "Ну, как твоя новая жизнь в качестве привидения?",
            "Похоже, ты нашел еще один способ проиграть.",
            "Надеюсь, на том свете есть инструкция по игре.",
            "Ты бы смог выиграть, если бы не это неудобное обстоятельство 'смерть'.",
            "Похоже, ты немного недооценил сложность игры.",
            "Единственный плюс смерти — на следующий день не нужно на работу.",
            "Сколько раз нужно умереть, чтобы научиться играть?",
            "Он был так стар, что умирать просто не имело смысла.",
            "Неудача — возможность узнать что-то новое",
            "Главное не задумываться, что было бы, если бы поступили иначе",
            "Смерть сродни сексу в старших классах – если б вы знали, сколько раз она вас миновала, то обалдели бы.",
            "Карл Маркс умер, Ленин умер... и мне чё-то нездоровится...",
            "— У Bomberman что, неприятности?\r\n— Нет, он мёртв.",
            "Никогда не думал, что меня прикончит воздушный шарик, сказали бы — не поверил."
        };
        public static string Citation
        {
            get
            {
                var rdn = new Random();
                int indexCitation = rdn.Next(citations.Length);
                return citations[indexCitation];
            }
        }
    }
}