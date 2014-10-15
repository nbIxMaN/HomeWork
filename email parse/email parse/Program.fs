module emailParse
open System.Text.RegularExpressions

let login = "([a-zA-Z0-9]+[\.\-\x5F]?){1,}[a-zA-Z0-9]"
let at = "@"
let lowLevelDomain = "([a-zA-Z0-9]+[\.\-\x5F]?){1,}[a-zA-Z0-9]"
let hightLevelDomain = "(\.[a-zA-Z]{2}|.aero|.asia|.biz|.cat|.com|.coop|.edu|.gov|.info|.int|.jobs|.mil|.mobi|.museum|.name|.net|.org|.pro|.tel|.travel|.xxx)"
let isValid email = Regex.IsMatch(email, "^" + login + at + lowLevelDomain + hightLevelDomain + "$")