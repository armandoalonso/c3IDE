using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using Action = c3IDE.Models.Action;

namespace c3IDE.Managers
{
    public static class ProjectManager
    {
        public static void WriteProject(C3Addon addon, string path)
        {
            SetupProjectDirectoy(path);
            WriteMetadataJson(addon, path);
            WriteAddonFiles(addon, path);

            switch (addon.Type)
            {
                case PluginType.SingleGlobalPlugin:
                case PluginType.DrawingPlugin:
                case PluginType.Behavior:

                    WriteActions(addon, path);
                    WriteConditions(addon, path);
                    WriteExpressions(addon, path);
                    WriteThirdPartyFile(addon, path);
                    WriteC2AddonFiles(addon, path);

                    break;
                case PluginType.Effect:

                    WriteEffectProperties(addon, path);
                    WriteEffectCode(addon, path);
                    WriteEffectParam(addon, path);
                    break;
                case PluginType.Theme:
                    WriteThemeCss(addon, path);
                    WriteThemeLanguage(addon, path);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            WriteIcon(addon, path);
        }

        public static C3Addon ReadProject(string path)
        {
            var fi = new FileInfo(path);

            var addon = new C3Addon();
            ReadMetadataJson(addon, fi.FullName);
            ReadAddonFiles(addon, fi.DirectoryName);

            switch (addon.Type)
            {
                case PluginType.SingleGlobalPlugin:
                case PluginType.DrawingPlugin:
                case PluginType.Behavior:

                    ReadActions(addon, fi.DirectoryName);
                    ReadConditions(addon, fi.DirectoryName);
                    ReadExpressions(addon, fi.DirectoryName);
                    ReadThirdPartyFile(addon, fi.DirectoryName);
                    ReadC2Addon(addon, fi.DirectoryName);

                    break;
                case PluginType.Effect:

                    ReadEffectProperties(addon, fi.DirectoryName);
                    ReadEffectCode(addon, fi.DirectoryName);
                    ReadEffectParam(addon, fi.DirectoryName);
                    break;

                case PluginType.Theme:
                    ReadThemeCss(addon, fi.DirectoryName);
                    ReadThemeLanguage(addon, fi.DirectoryName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ReadIcon(addon, fi.DirectoryName);
            return addon;
        }

        private static void SetupProjectDirectoy(string path)
        {
            //create project folder if not exists
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            DirectoryInfo di = new DirectoryInfo(path);

            //clear all files in directory
            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private static void WriteMetadataJson(C3Addon addon, string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@@METADATA");
            sb.AppendLine($"Id|{addon.Id}");
            sb.AppendLine($"Name|{addon.Name}");
            sb.AppendLine($"Class|{addon.Class}");
            sb.AppendLine($"Company|{addon.Company}");
            sb.AppendLine($"Author|{addon.Author}");
            sb.AppendLine($"MajorVersion|{addon.MajorVersion}");
            sb.AppendLine($"MinorVersion|{addon.MinorVersion}");
            sb.AppendLine($"RevisionVersion|{addon.RevisionVersion}");
            sb.AppendLine($"BuildVersion|{addon.BuildVersion}");
            sb.AppendLine($"Description|{addon.Description}");
            sb.AppendLine($"Category|{addon.AddonCategory}");
            sb.AppendLine($"Type|{addon.Type}");
            sb.AppendLine($"CreateDate|{addon.CreateDate}");
            sb.AppendLine($"DateModified|{addon.LastModified}");
            sb.AppendLine($"AddonId|{addon.AddonId}");

            File.WriteAllText(Path.Combine(path, $"{addon.Author}_{addon.Class}.c3ide"), sb.ToString());
        }

        private static void ReadMetadataJson(C3Addon addon, string fullpath)
        {
            try
            {
                var text = File.ReadAllLines(fullpath);
                addon.Id = Guid.Parse(text[1].Split('|')[1].Trim());
                addon.Name = text[2].Split('|')[1].Trim();
                addon.Class = text[3].Split('|')[1].Trim();
                addon.Company = text[4].Split('|')[1].Trim();
                addon.Author = text[5].Split('|')[1].Trim();

                addon.MajorVersion = int.Parse(text[6].Split('|')[1].Trim());
                addon.MinorVersion = int.Parse(text[7].Split('|')[1].Trim());
                addon.RevisionVersion = int.Parse(text[8].Split('|')[1].Trim());
                addon.BuildVersion = int.Parse(text[9].Split('|')[1].Trim());

                addon.Description = text[10].Split('|')[1].Trim();
                addon.AddonCategory = text[11].Split('|')[1].Trim();
                addon.Type = (PluginType)Enum.Parse(typeof(PluginType), text[12].Split('|')[1].Trim());
                addon.CreateDate = DateTime.Parse(text[13].Split('|')[1].Trim());
                addon.LastModified = DateTime.Parse(text[14].Split('|')[1].Trim());

                if (text.Length == 16)
                {
                    addon.AddonId = text[15].Split('|')[1].Trim();
                }
                else
                {
                    addon.AddonId = $"{addon.Author}_{addon.Class}";
                }

            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteAddonFiles(C3Addon addon, string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("@@START ADDON.JSON");
            sb.AppendLine(addon.AddonJson);
            sb.AppendLine("@@END ADDON.JSON");
            sb.AppendLine();

            sb.AppendLine("@@START PLUGIN_EDIT.JS");
            sb.AppendLine(addon.PluginEditTime);
            sb.AppendLine("@@END PLUGIN_EDIT.JS");
            sb.AppendLine();

            sb.AppendLine("@@START PLUGIN_RUN.JS");
            sb.AppendLine(addon.PluginRunTime);
            sb.AppendLine("@@END PLUGIN_RUN.JS");
            sb.AppendLine();

            sb.AppendLine("@@START TYPE_EDIT.JS");
            sb.AppendLine(addon.TypeEditTime);
            sb.AppendLine("@@END TYPE_EDIT.JS");
            sb.AppendLine();

            sb.AppendLine("@@START TYPE_RUN.JS");
            sb.AppendLine(addon.TypeRunTime);
            sb.AppendLine("@@END TYPE_RUN.JS");
            sb.AppendLine();

            sb.AppendLine("@@START INSTANCE_EDIT.JS");
            sb.AppendLine(addon.InstanceEditTime);
            sb.AppendLine("@@END INSTANCE_EDIT.JS");
            sb.AppendLine();

            sb.AppendLine("@@START INSTANCE_RUN.JS");
            sb.AppendLine(addon.InstanceRunTime);
            sb.AppendLine("@@END INSTANCE_RUN.JS");
            sb.AppendLine();

            sb.AppendLine("@@START LANG_PROP.JSON");
            sb.AppendLine(addon.LanguageProperties);
            sb.AppendLine("@@END LANG_PROP.JSON");
            sb.AppendLine();

            sb.AppendLine("@@START LANG_CAT.JSON");
            sb.AppendLine(addon.LanguageCategories);
            sb.AppendLine("@@END LANG_CAT.JSON");
            sb.AppendLine();


            File.WriteAllText(Path.Combine(path, "addon.c3idex"), sb.ToString());
        }

        private static void ReadAddonFiles(C3Addon addon, string path)
        {
            try
            {
                var text = File.ReadAllLines(Path.Combine(path, "addon.c3idex"));
                var state = ParserState.Idle;
                var section = string.Empty;
                StringBuilder temp = new StringBuilder();

                foreach (var line in text)
                {
                    //check for end
                    if (line.Contains("@@END"))
                    {
                        state = ParserState.End;
                        PopulateFile(addon, section, temp.ToString());
                    }

                    //check for parsing
                    if (state == ParserState.Start)
                    {
                        temp.AppendLine(line);
                    }

                    //check for start
                    if (line.Contains("@@START"))
                    {
                        section = line.Replace("@@START", string.Empty).Trim();
                        state = ParserState.Start;
                        temp = new StringBuilder();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteActions(C3Addon addon, string path)
        {
            var sb = new StringBuilder();
            foreach (var ace in addon.Actions)
            {
                sb.AppendLine($"@@START {ace.Key}|{ace.Value.Category}");
                sb.AppendLine("@@ACE");
                sb.AppendLine(ace.Value.Ace);

                sb.AppendLine("@@LANG");
                sb.AppendLine(ace.Value.Language);

                sb.AppendLine("@@CODE");
                sb.AppendLine(ace.Value.Code);

                sb.AppendLine($"@@END {ace.Key}");
                sb.AppendLine();
            }

            File.WriteAllText(Path.Combine(path, "actions.c3idex"), sb.ToString());
        }

        private static void ReadActions(C3Addon addon, string path)
        {
            addon.Actions = new Dictionary<string, Action>();
            try
            {
                var text = File.ReadAllLines(Path.Combine(path, "actions.c3idex"));
                var state = ParserState.Idle;
                var id = string.Empty;
                var category = string.Empty;
                var section = string.Empty;
                var temp = new Dictionary<string, StringBuilder>();

                foreach (var line in text)
                {
                    //check for end
                    if (line.Contains("@@END"))
                    {
                        state = ParserState.End;
                        PopulateAction(addon, id, category, temp["ACE"].ToString(), temp["LANG"].ToString(), temp["CODE"].ToString());
                    }


                    //check for parsing
                    if (state == ParserState.Start)
                    {
                        if (line.Contains("@@ACE"))
                        {
                            section = "ACE";
                        }
                        else if (line.Contains("@@LANG"))
                        {
                            section = "LANG";
                        }
                        else if (line.Contains("@@CODE"))
                        {
                            section = "CODE";
                        }
                        else
                        {
                            temp[section].AppendLine(line);
                        }
                    }

                    //check for start
                    if (line.Contains("@@START"))
                    {
                        id = line.Replace("@@START", string.Empty).Trim().Split('|')[0];
                        category = line.Replace("@@START", string.Empty).Trim().Split('|')[1];

                        state = ParserState.Start;
                        temp = new Dictionary<string, StringBuilder>
                        {
                            {"ACE", new StringBuilder()},
                            {"LANG", new StringBuilder()},
                            {"CODE", new StringBuilder()}
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteConditions(C3Addon addon, string path)
        {
            var sb = new StringBuilder();
            foreach (var ace in addon.Conditions)
            {
                sb.AppendLine($"@@START {ace.Key}|{ace.Value.Category}");
                sb.AppendLine("@@ACE");
                sb.AppendLine(ace.Value.Ace);

                sb.AppendLine("@@LANG");
                sb.AppendLine(ace.Value.Language);

                sb.AppendLine("@@CODE");
                sb.AppendLine(ace.Value.Code);

                sb.AppendLine($"@@END {ace.Key}");
                sb.AppendLine();
            }

            File.WriteAllText(Path.Combine(path, "conditions.c3idex"), sb.ToString());
        }

        private static void ReadConditions(C3Addon addon, string path)
        {
            addon.Conditions = new Dictionary<string, Condition>();
            try
            {
                var text = File.ReadAllLines(Path.Combine(path, "conditions.c3idex"));
                var state = ParserState.Idle;
                var id = string.Empty;
                var category = string.Empty;
                var section = string.Empty;
                var temp = new Dictionary<string, StringBuilder>();

                foreach (var line in text)
                {
                    //check for end
                    if (line.Contains("@@END"))
                    {
                        state = ParserState.End;
                        PopulateCondition(addon, id, category, temp["ACE"].ToString(), temp["LANG"].ToString(), temp["CODE"].ToString());
                    }

                    //check for parsing
                    if (state == ParserState.Start)
                    {
                        if (line.Contains("@@ACE"))
                        {
                            section = "ACE";
                        }
                        else if (line.Contains("@@LANG"))
                        {
                            section = "LANG";
                        }
                        else if (line.Contains("@@CODE"))
                        {
                            section = "CODE";
                        }
                        else
                        {
                            temp[section].AppendLine(line);
                        }
                    }

                    //check for start
                    if (line.Contains("@@START"))
                    {
                        id = line.Replace("@@START", string.Empty).Trim().Split('|')[0];
                        category = line.Replace("@@START", string.Empty).Trim().Split('|')[1];

                        state = ParserState.Start;
                        temp = new Dictionary<string, StringBuilder>
                        {
                            {"ACE", new StringBuilder()},
                            {"LANG", new StringBuilder()},
                            {"CODE", new StringBuilder()}
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteExpressions(C3Addon addon, string path)
        {
            var sb = new StringBuilder();
            foreach (var ace in addon.Expressions)
            {
                sb.AppendLine($"@@START {ace.Key}|{ace.Value.Category}");
                sb.AppendLine("@@ACE");
                sb.AppendLine(ace.Value.Ace);

                sb.AppendLine("@@LANG");
                sb.AppendLine(ace.Value.Language);

                sb.AppendLine("@@CODE");
                sb.AppendLine(ace.Value.Code);

                sb.AppendLine($"@@END {ace.Key}");
                sb.AppendLine();
            }

            File.WriteAllText(Path.Combine(path, "expressions.c3idex"), sb.ToString());
        }

        private static void ReadExpressions(C3Addon addon, string path)
        {
            addon.Expressions = new Dictionary<string, Expression>();
            try
            {
                var text = File.ReadAllLines(Path.Combine(path, "expressions.c3idex"));
                var state = ParserState.Idle;
                var id = string.Empty;
                var category = string.Empty;
                var section = string.Empty;
                var temp = new Dictionary<string, StringBuilder>();

                foreach (var line in text)
                {
                    //check for end
                    if (line.Contains("@@END"))
                    {
                        state = ParserState.End;
                        PopulateExpression(addon, id, category, temp["ACE"].ToString(), temp["LANG"].ToString(), temp["CODE"].ToString());
                    }

                    //check for parsing
                    if (state == ParserState.Start)
                    {
                        if (line.Contains("@@ACE"))
                        {
                            section = "ACE";
                        }
                        else if (line.Contains("@@LANG"))
                        {
                            section = "LANG";
                        }
                        else if (line.Contains("@@CODE"))
                        {
                            section = "CODE";
                        }
                        else
                        {
                            temp[section].AppendLine(line);
                        }
                    }

                    //check for start
                    if (line.Contains("@@START"))
                    {
                        id = line.Replace("@@START", string.Empty).Trim().Split('|')[0];
                        category = line.Replace("@@START", string.Empty).Trim().Split('|')[1];

                        state = ParserState.Start;
                        temp = new Dictionary<string, StringBuilder>
                        {
                            {"ACE", new StringBuilder()},
                            {"LANG", new StringBuilder()},
                            {"CODE", new StringBuilder()}
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteEffectProperties(C3Addon addon, string path)
        {
            var sb = new StringBuilder();
            if (addon.Effect != null)
            {
                sb.AppendLine($"BlendsBackground|{addon.Effect.BlendsBackground}");
                sb.AppendLine($"CrossSampling|{addon.Effect.CrossSampling}");
                sb.AppendLine($"PreservesOpaqueness|{addon.Effect.PreservesOpaqueness}");
                sb.AppendLine($"Animated|{addon.Effect.Animated}");
                sb.AppendLine($"MustPredraw|{addon.Effect.MustPredraw}");
                sb.AppendLine($"ExtendBoxHorizontal|{addon.Effect.ExtendBoxHorizontal}");
                sb.AppendLine($"ExtendBoxVertical|{addon.Effect.ExtendBoxVertical}");
            }
            File.WriteAllText(Path.Combine(path, $"effect_properties.c3idex"), sb.ToString());
        }

        private static void ReadEffectProperties(C3Addon addon, string path)
        {
            addon.Effect = new Effect();
            try
            {
                var text = File.ReadAllLines(Path.Combine(path, "effect_properties.c3idex"));
                if(!text.Any()) return;
                addon.Effect.BlendsBackground = bool.Parse(text[0].Split('|')[1].Trim().ToLower());
                addon.Effect.CrossSampling = bool.Parse(text[1].Split('|')[1].Trim().ToLower());
                addon.Effect.PreservesOpaqueness = bool.Parse(text[2].Split('|')[1].Trim().ToLower());
                addon.Effect.Animated = bool.Parse(text[3].Split('|')[1].Trim().ToLower());
                addon.Effect.MustPredraw = bool.Parse(text[4].Split('|')[1].Trim().ToLower());
                addon.Effect.ExtendBoxHorizontal = int.Parse(text[5].Split('|')[1].Trim());
                addon.Effect.ExtendBoxVertical = int.Parse(text[6].Split('|')[1].Trim());
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }         
        }

        private static void WriteEffectCode(C3Addon addon, string path)
        {
            var code = string.Empty;
            if (addon.Effect != null)
            {
                code = addon.Effect.Code;
            }
            File.WriteAllText(Path.Combine(path, $"effect_code.c3idex"), code);
        }

        private static void ReadEffectCode(C3Addon addon, string path)
        {
            try
            {
                var text = File.ReadAllText(Path.Combine(path, "effect_code.c3idex"));
                if (!text.Any()) return;
                addon.Effect.Code = text;
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteEffectParam(C3Addon addon, string path)
        {
            var sb = new StringBuilder();
            if (addon.Effect != null)
            {
                foreach (var param in addon.Effect.Parameters)
                {
                    sb.AppendLine($"@@START {param.Key}|{param.Value.VariableDeclaration}");
                    sb.AppendLine("@@JSON");
                    sb.AppendLine(param.Value.Json);

                    sb.AppendLine("@@LANG");
                    sb.AppendLine(param.Value.Lang);

                    sb.AppendLine($"@@END {param.Key}");
                    sb.AppendLine();
                }
            }
               
            File.WriteAllText(Path.Combine(path, "effect_parameters.c3idex"), sb.ToString());
        }

        private static void ReadEffectParam(C3Addon addon, string path)
        {
            addon.Effect.Parameters = new Dictionary<string, EffectParameter>();
            try
            {
                var text = File.ReadAllLines(Path.Combine(path, "effect_parameters.c3idex"));
                var state = ParserState.Idle;
                var id = string.Empty;
                var declaration = string.Empty;
                var section = string.Empty;
                var temp = new Dictionary<string, StringBuilder>();

                foreach (var line in text)
                {
                    //check for end
                    if (line.Contains("@@END"))
                    {
                        state = ParserState.End;
                        PopulateEffectParam(addon, id, declaration, temp["JSON"].ToString(), temp["LANG"].ToString());
                    }

                    //check for parsing
                    if (state == ParserState.Start)
                    {
                        if (line.Contains("@@JSON"))
                        {
                            section = "JSON";
                        }
                        else if (line.Contains("@@LANG"))
                        {
                            section = "LANG";
                        }
                        else
                        {
                            temp[section].AppendLine(line);
                        }
                    }

                    //check for start
                    if (line.Contains("@@START"))
                    {
                        id = line.Replace("@@START", string.Empty).Trim().Split('|')[0];
                        declaration = line.Replace("@@START", string.Empty).Trim().Split('|')[1];

                        state = ParserState.Start;
                        temp = new Dictionary<string, StringBuilder>
                        {
                            {"JSON", new StringBuilder()},
                            {"LANG", new StringBuilder()}
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteThirdPartyFile(C3Addon addon, string path)
        {
            var sb = new StringBuilder();
            if (addon.ThirdPartyFiles.Any())
            {
                foreach (var file in addon.ThirdPartyFiles)
                {
                    sb.AppendLine($"@@START {file.Key}|{file.Value.Extention}|{file.Value.Rootfolder.ToString().ToLower()}|{file.Value.C3Folder.ToString().ToLower()}|{file.Value.C2Folder.ToString().ToLower()}|{file.Value.FileType}|{file.Value.Compress.ToString()}|{file.Value.PlainText.ToString()}");

                    sb.AppendLine("@@TEMPLATE");
                    sb.AppendLine(file.Value.PluginTemplate);

                    sb.AppendLine("@@CONTENT");
                    sb.AppendLine(file.Value.Content);

                    sb.AppendLine("@@BYTES");
                    if (file.Value.Bytes != null)
                    {
                        sb.AppendLine(Convert.ToBase64String(file.Value.Bytes));
                    }

                    sb.AppendLine($"@@END {file.Key}");
                    sb.AppendLine();
                }
            }

            File.WriteAllText(Path.Combine(path, "third_party_files.c3idex"), sb.ToString());
        }

        private static void ReadThirdPartyFile(C3Addon addon, string path)
        {
            addon.ThirdPartyFiles = new Dictionary<string, ThirdPartyFile>();
            try
            {
                var text = File.ReadAllLines(Path.Combine(path, "third_party_files.c3idex"));
                var state = ParserState.Idle;
                var file = string.Empty;
                var extention = string.Empty;
                var section = string.Empty;
                var root = string.Empty;
                var c3 = string.Empty;
                var c2 = string.Empty;
                var fileType = string.Empty;
                var compress = string.Empty;
                var plainText = string.Empty;
                var temp = new Dictionary<string, StringBuilder>();


                foreach (var line in text)
                {
                    //check for end
                    if (line.Contains("@@END"))
                    {
                        state = ParserState.End;
                        PopulateThridPartyFile(addon, file, extention, temp["TEMPLATE"].ToString(), temp["CONTENT"].ToString(), temp["BYTES"].ToString(), root, c3, c2, fileType, compress, plainText);
                    }

                    //check for parsing
                    if (state == ParserState.Start)
                    {
                        if (line.Contains("@@TEMPLATE"))
                        {
                            section = "TEMPLATE";
                        }
                        else if (line.Contains("@@CONTENT"))
                        {
                            section = "CONTENT";
                        }
                        else if (line.Contains("@@BYTES"))
                        {
                            section = "BYTES";
                        }
                        else
                        {
                            temp[section].AppendLine(line);
                        }
                    }

                    //check for start
                    if (line.Contains("@@START"))
                    {
                        try
                        {
                            var properties = line.Replace("@@START", string.Empty).Trim().Split('|');

                            file = properties[0];
                            extention = properties[1];
                            root = properties.Length > 2 ? properties[2] : " ";
                            c3 = properties.Length > 3 ? properties[3] : " ";
                            c2 = properties.Length > 4 ? properties[4] : " ";
                            fileType = properties.Length > 5 ? properties[5] : " ";
                            compress = properties.Length > 6 ? properties[6] : " ";
                        }
                        catch (Exception ex)
                        {
                            LogManager.AddImportLogMessage(ex.Message);
                            //ignore handle properties not being there 
                        }


                        state = ParserState.Start;
                        temp = new Dictionary<string, StringBuilder>
                        {
                            {"TEMPLATE", new StringBuilder()},
                            {"CONTENT", new StringBuilder()},
                            {"BYTES", new StringBuilder()}
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteThemeLanguage(C3Addon addon, string path)
        {
            var lang = string.Empty;
            if (addon.ThemeLangauge != null)
            {
                lang = addon.ThemeLangauge;
            }
            File.WriteAllText(Path.Combine(path, $"theme_lang.c3idex"), lang);
        }

        private static void ReadThemeLanguage(C3Addon addon, string path)
        {
            try
            {
                var text = File.ReadAllText(Path.Combine(path, "theme_lang.c3idex"));
                if (!text.Any()) return;
                addon.ThemeLangauge = text;
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteThemeCss(C3Addon addon, string path)
        {
            var css = string.Empty;
            if (addon.ThemeCss != null)
            {
                css = addon.ThemeCss;
            }
            File.WriteAllText(Path.Combine(path, $"theme_css.c3idex"), css);
        }

        private static void ReadThemeCss(C3Addon addon, string path)
        {
            try
            {
                var text = File.ReadAllText(Path.Combine(path, "theme_css.c3idex"));
                if (!text.Any()) return;
                addon.ThemeCss = text;
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void WriteIcon(C3Addon addon, string path)
        {
            File.WriteAllText(Path.Combine(path, "icon.svg"), addon.IconXml);
        }

        private static void ReadIcon(C3Addon addon, string path)
        {
            addon.IconXml = File.ReadAllText(Path.Combine(path, "icon.svg"));
        }

        private static void WriteC2AddonFiles(C3Addon addon, string path)
        {
            //break if no c2 data is defined
            if(string.IsNullOrWhiteSpace(addon.C2RunTime)) return;

            var sb = new StringBuilder();

            sb.AppendLine("@@START C2RUNTIME.JS");
            sb.AppendLine(addon.C2RunTime);
            sb.AppendLine("@@END C2RUNTIME.JS");
            sb.AppendLine();

            File.WriteAllText(Path.Combine(path, "c2runtime.c3idex"), sb.ToString());
        }

        private static void ReadC2Addon(C3Addon addon, string path)
        {
            try
            {
                //break if there is no c2runtime file
                if (!File.Exists(Path.Combine(path, "c2runtime.c3idex"))) return;

                var text = File.ReadAllLines(Path.Combine(path, "c2runtime.c3idex"));
                var state = ParserState.Idle;
                var section = string.Empty;
                StringBuilder temp = new StringBuilder();

                foreach (var line in text)
                {
                    //check for end
                    if (line.Contains("@@END"))
                    {
                        state = ParserState.End;
                        PopulateFile(addon, section, temp.ToString());
                    }

                    //check for parsing
                    if (state == ParserState.Start)
                    {
                        temp.AppendLine(line);
                    }

                    //check for start
                    if (line.Contains("@@START"))
                    {
                        section = line.Replace("@@START", string.Empty).Trim();
                        state = ParserState.Start;
                        temp = new StringBuilder();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        private static void PopulateThridPartyFile(C3Addon addon, string file, string extention, string template, string content, string bytes, string root, string c3, string c2, string filetype, string compress, string plainText)
        {
            addon.ThirdPartyFiles.Add(file, new ThirdPartyFile
            {
                FileName = file,
                Extention = string.IsNullOrWhiteSpace(extention) ? Path.GetExtension(file) : extention,
                PluginTemplate = template,
                Content = content,
                Bytes = Convert.FromBase64String(bytes),
                Rootfolder = root == "true",
                C3Folder = c3 == "true",
                C2Folder = c2 == "true",
                FileType = string.IsNullOrWhiteSpace(filetype) ? "inline-script" : filetype,
                Compress = compress == "true",
                PlainText = plainText == "true"
            });
        }

        private static void PopulateFile(C3Addon addon, string section, string content)
        {
            switch (section)
            {
                case "ADDON.JSON": addon.AddonJson = content; break;
                case "PLUGIN_EDIT.JS": addon.PluginEditTime = content; break;
                case "PLUGIN_RUN.JS": addon.PluginRunTime = content; break;
                case "TYPE_EDIT.JS": addon.TypeEditTime = content; break;
                case "TYPE_RUN.JS": addon.TypeRunTime = content; break;
                case "INSTANCE_EDIT.JS": addon.InstanceEditTime = content; break;
                case "INSTANCE_RUN.JS": addon.InstanceRunTime = content; break;
                case "LANG_PROP.JSON": addon.LanguageProperties = content; break;
                case "LANG_CAT.JSON": addon.LanguageCategories = content; break;
                case "C2RUNTIME.JS": addon.C2RunTime = content; break;
            }
        }

        private static void PopulateAction(C3Addon addon, string id, string category, string ace, string lang, string code)
        {
            addon.Actions.Add(id, new Models.Action
            {
                Id = id,
                Ace = ace,
                Language = lang,
                Code = code,
                Category = category
            });
        }

        private static void PopulateCondition(C3Addon addon, string id, string category, string ace, string lang, string code)
        {
            addon.Conditions.Add(id, new Models.Condition()
            {
                Id = id,
                Ace = ace,
                Language = lang,
                Code = code,
                Category = category
            });
        }

        private static void PopulateExpression(C3Addon addon, string id, string category, string ace, string lang, string code)
        {
            addon.Expressions.Add(id, new Models.Expression()
            {
                Id = id,
                Ace = ace,
                Language = lang,
                Code = code,
                Category = category
            });
        }

        private static void PopulateEffectParam(C3Addon addon, string id, string declaration, string json, string lang)
        {
            addon.Effect.Parameters.Add(id, new EffectParameter()
            {
                Key = id,
                VariableDeclaration = declaration,
                Json = json,
                Lang = lang
            });
        }

        private enum ParserState
        {
            Idle,
            Start,
            End
        }
    }
}
