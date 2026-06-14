using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 中文汉字转拼音工具类（轻量内置字典，覆盖常用 200+ 汉字，用于搜索索引、排序键等场景）
/// </summary>
public static class PinyinUtil
{
    /// <summary>
    /// 汉字 -> 拼音映射表（单字级别简化映射，非全量拼音库）
    /// </summary>
    private static readonly Dictionary<char, string> PinyinMap = new()
    {
        ['啊'] = "a", ['阿'] = "a", ['哎'] = "ai", ['哀'] = "ai", ['安'] = "an", ['按'] = "an",
        ['吧'] = "ba", ['八'] = "ba", ['把'] = "ba", ['爸'] = "ba", ['百'] = "bai", ['白'] = "bai",
        ['班'] = "ban", ['般'] = "ban", ['办'] = "ban", ['包'] = "bao", ['报'] = "bao", ['保'] = "bao",
        ['被'] = "bei", ['北'] = "bei", ['本'] = "ben", ['泵'] = "beng", ['比'] = "bi", ['笔'] = "bi",
        ['部'] = "bu", ['不'] = "bu", ['才'] = "cai", ['材'] = "cai", ['采'] = "cai", ['参'] = "can",
        ['仓'] = "cang", ['藏'] = "cang", ['操'] = "cao", ['草'] = "cao", ['测'] = "ce", ['策'] = "ce",
        ['查'] = "cha", ['差'] = "cha", ['产'] = "chan", ['长'] = "chang", ['常'] = "chang", ['厂'] = "chang",
        ['超'] = "chao", ['朝'] = "chao", ['车'] = "che", ['彻'] = "che", ['陈'] = "chen", ['称'] = "cheng",
        ['成'] = "cheng", ['程'] = "cheng", ['城'] = "cheng", ['出'] = "chu", ['处'] = "chu", ['触'] = "chu",
        ['传'] = "chuan", ['船'] = "chuan", ['窗'] = "chuang", ['创'] = "chuang", ['春'] = "chun", ['纯'] = "chun",
        ['词'] = "ci", ['此'] = "ci", ['次'] = "ci", ['从'] = "cong", ['存'] = "cun", ['错'] = "cuo",
        ['达'] = "da", ['打'] = "da", ['大'] = "da", ['代'] = "dai", ['带'] = "dai", ['单'] = "dan",
        ['但'] = "dan", ['弹'] = "dan", ['当'] = "dang", ['档'] = "dang", ['到'] = "dao", ['道'] = "dao",
        ['导'] = "dao", ['的'] = "de", ['得'] = "de", ['等'] = "deng", ['灯'] = "deng", ['低'] = "di",
        ['地'] = "di", ['第'] = "di", ['点'] = "dian", ['电'] = "dian", ['店'] = "dian", ['定'] = "ding",
        ['顶'] = "ding", ['东'] = "dong", ['冬'] = "dong", ['动'] = "dong", ['都'] = "du", ['读'] = "du",
        ['度'] = "du", ['端'] = "duan", ['短'] = "duan", ['段'] = "duan", ['对'] = "dui", ['队'] = "dui",
        ['多'] = "duo", ['多'] = "duo", ['而'] = "er", ['儿'] = "er", ['发'] = "fa", ['法'] = "fa",
        ['反'] = "fan", ['方'] = "fang", ['房'] = "fang", ['放'] = "fang", ['非'] = "fei", ['飞'] = "fei",
        ['分'] = "fen", ['份'] = "fen", ['风'] = "feng", ['封'] = "feng", ['夫'] = "fu", ['服'] = "fu",
        ['福'] = "fu", ['附'] = "fu", ['复'] = "fu", ['该'] = "gai", ['改'] = "gai", ['干'] = "gan",
        ['感'] = "gan", ['刚'] = "gang", ['钢'] = "gang", ['高'] = "gao", ['告'] = "gao", ['哥'] = "ge",
        ['格'] = "ge", ['各'] = "ge", ['给'] = "gei", ['根'] = "gen", ['更'] = "geng", ['工'] = "gong",
        ['公'] = "gong", ['功'] = "gong", ['共'] = "gong", ['构'] = "gou", ['够'] = "gou", ['古'] = "gu",
        ['故'] = "gu", ['顾'] = "gu", ['关'] = "guan", ['管'] = "guan", ['观'] = "guan", ['广'] = "guang",
        ['规'] = "gui", ['归'] = "gui", ['国'] = "guo", ['果'] = "guo", ['过'] = "guo", ['还'] = "hai",
        ['海'] = "hai", ['含'] = "han", ['好'] = "hao", ['号'] = "hao", ['和'] = "he", ['合'] = "he",
        ['何'] = "he", ['黑'] = "hei", ['很'] = "hen", ['红'] = "hong", ['后'] = "hou", ['候'] = "hou",
        ['湖'] = "hu", ['互'] = "hu", ['户'] = "hu", ['化'] = "hua", ['话'] = "hua", ['划'] = "hua",
        ['还'] = "hai", ['坏'] = "huai", ['环'] = "huan", ['换'] = "huan", ['回'] = "hui", ['会'] = "hui",
        ['活'] = "huo", ['火'] = "huo", ['或'] = "huo", ['机'] = "ji", ['几'] = "ji", ['及'] = "ji",
        ['级'] = "ji", ['计'] = "ji", ['记'] = "ji", ['技'] = "ji", ['继'] = "ji", ['家'] = "jia",
        ['加'] = "jia", ['价'] = "jia", ['间'] = "jian", ['检'] = "jian", ['简'] = "jian", ['建'] = "jian",
        ['件'] = "jian", ['将'] = "jiang", ['江'] = "jiang", ['讲'] = "jiang", ['交'] = "jiao", ['教'] = "jiao",
        ['角'] = "jiao", ['叫'] = "jiao", ['接'] = "jie", ['结'] = "jie", ['解'] = "jie", ['界'] = "jie",
        ['今'] = "jin", ['金'] = "jin", ['仅'] = "jin", ['进'] = "jin", ['近'] = "jin", ['经'] = "jing",
        ['精'] = "jing", ['净'] = "jing", ['九'] = "jiu", ['就'] = "jiu", ['旧'] = "jiu", ['据'] = "ju",
        ['具'] = "ju", ['距'] = "ju", ['卷'] = "juan", ['决'] = "jue", ['觉'] = "jue", ['开'] = "kai",
        ['看'] = "kan", ['刊'] = "kan", ['考'] = "kao", ['科'] = "ke", ['可'] = "ke", ['客'] = "ke",
        ['空'] = "kong", ['控'] = "kong", ['口'] = "kou", ['库'] = "ku", ['快'] = "kuai", ['款'] = "kuan",
        ['况'] = "kuang", ['来'] = "lai", ['老'] = "lao", ['了'] = "le", ['累'] = "lei", ['类'] = "lei",
        ['冷'] = "leng", ['里'] = "li", ['理'] = "li", ['力'] = "li", ['历'] = "li", ['立'] = "li",
        ['利'] = "li", ['联'] = "lian", ['连'] = "lian", ['量'] = "liang", ['两'] = "liang", ['了'] = "liao",
        ['料'] = "liao", ['列'] = "lie", ['领'] = "ling", ['令'] = "ling", ['流'] = "liu", ['六'] = "liu",
        ['龙'] = "long", ['录'] = "lu", ['路'] = "lu", ['率'] = "lv", ['旅'] = "lv", ['律'] = "lv",
        ['论'] = "lun", ['落'] = "luo", ['马'] = "ma", ['码'] = "ma", ['买'] = "mai", ['迈'] = "mai",
        ['满'] = "man", ['毛'] = "mao", ['么'] = "me", ['没'] = "mei", ['每'] = "mei", ['美'] = "mei",
        ['门'] = "men", ['们'] = "men", ['梦'] = "meng", ['米'] = "mi", ['密'] = "mi", ['面'] = "mian",
        ['民'] = "min", ['名'] = "ming", ['明'] = "ming", ['命'] = "ming", ['模'] = "mo", ['末'] = "mo",
        ['某'] = "mou", ['目'] = "mu", ['那'] = "na", ['难'] = "nan", ['南'] = "nan", ['内'] = "nei",
        ['能'] = "neng", ['你'] = "ni", ['年'] = "nian", ['念'] = "nian", ['鸟'] = "niao", ['您'] = "nin",
        ['牛'] = "niu", ['农'] = "nong", ['女'] = "nv", ['欧'] = "ou", ['偶'] = "ou", ['排'] = "pai",
        ['派'] = "pai", ['判'] = "pan", ['旁'] = "pang", ['培'] = "pei", ['配'] = "pei", ['朋'] = "peng",
        ['批'] = "pi", ['片'] = "pian", ['票'] = "piao", ['品'] = "pin", ['平'] = "ping", ['评'] = "ping",
        ['凭'] = "ping", ['破'] = "po", ['普'] = "pu", ['期'] = "qi", ['其'] = "qi", ['起'] = "qi",
        ['气'] = "qi", ['器'] = "qi", ['千'] = "qian", ['前'] = "qian", ['钱'] = "qian", ['强'] = "qiang",
        ['墙'] = "qiang", ['且'] = "qie", ['亲'] = "qin", ['青'] = "qing", ['清'] = "qing", ['情'] = "qing",
        ['请'] = "qing", ['秋'] = "qiu", ['求'] = "qiu", ['区'] = "qu", ['取'] = "qu", ['全'] = "quan",
        ['权'] = "quan", ['却'] = "que", ['然'] = "ran", ['让'] = "rang", ['人'] = "ren", ['认'] = "ren",
        ['任'] = "ren", ['日'] = "ri", ['容'] = "rong", ['如'] = "ru", ['入'] = "ru", ['软'] = "ruan",
        ['三'] = "san", ['散'] = "san", ['色'] = "se", ['山'] = "shan", ['商'] = "shang", ['上'] = "shang",
        ['少'] = "shao", ['社'] = "she", ['设'] = "she", ['申'] = "shen", ['身'] = "shen", ['深'] = "shen",
        ['什'] = "shen", ['生'] = "sheng", ['声'] = "sheng", ['省'] = "sheng", ['十'] = "shi", ['时'] = "shi",
        ['实'] = "shi", ['识'] = "shi", ['使'] = "shi", ['是'] = "shi", ['市'] = "shi", ['示'] = "shi",
        ['式'] = "shi", ['事'] = "shi", ['势'] = "shi", ['收'] = "shou", ['手'] = "shou", ['首'] = "shou",
        ['受'] = "shou", ['书'] = "shu", ['术'] = "shu", ['数'] = "shu", ['树'] = "shu", ['双'] = "shuang",
        ['水'] = "shui", ['说'] = "shuo", ['思'] = "si", ['司'] = "si", ['四'] = "si", ['送'] = "song",
        ['诉'] = "su", ['速'] = "su", ['算'] = "suan", ['虽'] = "sui", ['随'] = "sui", ['所'] = "suo",
        ['他'] = "ta", ['它'] = "ta", ['太'] = "tai", ['态'] = "tai", ['谈'] = "tan", ['弹'] = "tan",
        ['特'] = "te", ['提'] = "ti", ['体'] = "ti", ['天'] = "tian", ['条'] = "tiao", ['调'] = "tiao",
        ['听'] = "ting", ['通'] = "tong", ['同'] = "tong", ['统'] = "tong", ['头'] = "tou", ['图'] = "tu",
        ['团'] = "tuan", ['推'] = "tui", ['退'] = "tui", ['外'] = "wai", ['完'] = "wan", ['万'] = "wan",
        ['网'] = "wang", ['往'] = "wang", ['为'] = "wei", ['位'] = "wei", ['文'] = "wen", ['问'] = "wen",
        ['我'] = "wo", ['无'] = "wu", ['五'] = "wu", ['物'] = "wu", ['务'] = "wu", ['西'] = "xi",
        ['习'] = "xi", ['系'] = "xi", ['细'] = "xi", ['下'] = "xia", ['先'] = "xian", ['显'] = "xian",
        ['现'] = "xian", ['线'] = "xian", ['相'] = "xiang", ['想'] = "xiang", ['向'] = "xiang", ['项'] = "xiang",
        ['消'] = "xiao", ['小'] = "xiao", ['效'] = "xiao", ['校'] = "xiao", ['些'] = "xie", ['协'] = "xie",
        ['写'] = "xie", ['谢'] = "xie", ['心'] = "xin", ['新'] = "xin", ['信'] = "xin", ['兴'] = "xing",
        ['星'] = "xing", ['行'] = "xing", ['形'] = "xing", ['性'] = "xing", ['修'] = "xiu", ['需'] = "xu",
        ['许'] = "xu", ['序'] = "xu", ['选'] = "xuan", ['学'] = "xue", ['血'] = "xue", ['询'] = "xun",
        ['压'] = "ya", ['亚'] = "ya", ['研'] = "yan", ['验'] = "yan", ['言'] = "yan", ['阳'] = "yang",
        ['样'] = "yang", ['要'] = "yao", ['药'] = "yao", ['也'] = "ye", ['业'] = "ye", ['页'] = "ye",
        ['一'] = "yi", ['医'] = "yi", ['依'] = "yi", ['已'] = "yi", ['以'] = "yi", ['意'] = "yi",
        ['义'] = "yi", ['因'] = "yin", ['引'] = "yin", ['应'] = "ying", ['英'] = "ying", ['营'] = "ying",
        ['用'] = "yong", ['永'] = "yong", ['由'] = "you", ['有'] = "you", ['又'] = "you", ['右'] = "you",
        ['于'] = "yu", ['与'] = "yu", ['语'] = "yu", ['员'] = "yuan", ['原'] = "yuan", ['远'] = "yuan",
        ['院'] = "yuan", ['愿'] = "yuan", ['月'] = "yue", ['越'] = "yue", ['云'] = "yun", ['运'] = "yun",
        ['在'] = "zai", ['再'] = "zai", ['早'] = "zao", ['造'] = "zao", ['则'] = "ze", ['增'] = "zeng",
        ['展'] = "zhan", ['站'] = "zhan", ['战'] = "zhan", ['张'] = "zhang", ['章'] = "zhang", ['长'] = "zhang",
        ['找'] = "zhao", ['照'] = "zhao", ['者'] = "zhe", ['这'] = "zhe", ['真'] = "zhen", ['正'] = "zheng",
        ['整'] = "zheng", ['政'] = "zheng", ['之'] = "zhi", ['支'] = "zhi", ['知'] = "zhi", ['直'] = "zhi",
        ['值'] = "zhi", ['只'] = "zhi", ['指'] = "zhi", ['至'] = "zhi", ['制'] = "zhi", ['质'] = "zhi",
        ['中'] = "zhong", ['种'] = "zhong", ['重'] = "zhong", ['周'] = "zhou", ['主'] = "zhu", ['住'] = "zhu",
        ['注'] = "zhu", ['抓'] = "zhua", ['专'] = "zhuan", ['转'] = "zhuan", ['装'] = "zhuang", ['状'] = "zhuang",
        ['追'] = "zhui", ['准'] = "zhun", ['着'] = "zhuo", ['资'] = "zi", ['子'] = "zi", ['字'] = "zi",
        ['自'] = "zi", ['总'] = "zong", ['走'] = "zou", ['租'] = "zu", ['足'] = "zu", ['族'] = "zu",
        ['组'] = "zu", ['最'] = "zui", ['昨'] = "zuo", ['左'] = "zuo", ['作'] = "zuo", ['做'] = "zuo",
        ['座'] = "zuo"
    };

    /// <summary>
    /// 将字符串中的汉字转换为完整拼音（非汉字字符原样保留）
    /// </summary>
    /// <param name="text">原始文本</param>
    /// <returns>拼音化字符串</returns>
    public static string ToPinyin(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        var sb = new StringBuilder();
        foreach (var ch in text)
        {
            if (PinyinMap.TryGetValue(ch, out var pinyin))
                sb.Append(pinyin);
            else
                sb.Append(ch);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 将字符串中的汉字转换为拼音首字母（非汉字字符原样保留）
    /// </summary>
    /// <param name="text">原始文本</param>
    /// <returns>拼音首字母字符串</returns>
    public static string ToPinyinInitials(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        var sb = new StringBuilder();
        foreach (var ch in text)
        {
            if (PinyinMap.TryGetValue(ch, out var pinyin))
                sb.Append(pinyin[0]);
            else
                sb.Append(ch);
        }
        return sb.ToString();
    }
}
