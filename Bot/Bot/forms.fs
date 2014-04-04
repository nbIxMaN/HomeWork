module FormExample

open System.Windows.Forms
open System.Drawing
open AIMLbot

let bot_loading = 
    let bot = new Bot()
    bot.loadSettings()
    let user = new User("user", bot)
    bot.isAcceptingUserInput = false
    bot.loadAIMLFromFiles()
    bot.isAcceptingUserInput = true
    bot

let user_loading bot = new User("user", bot)

let bot_talking user bot text = 
    let r = new Request (text, user, bot)
    let res = bot.Chat(r)
    res.Output

let read_user_text() =
    let bot = bot_loading
    let user = user_loading (bot)
    let rec readX' k = 
        let frm = new Form()
        let size = new Size (100, 100)
        let text = new Label(Text = k, Top = 10, Size = size )
        let tb = new TextBox(Text = "", Top = 170)
        let btOk = new Button(Text="Ok", Top = 200)
        let btClose = new Button(Text="Close", Top = 200, Left = 100)
        btOk.Click.Add (fun _ -> let x = string tb.Text
                                 let k = bot_talking user bot x
                                 frm.Close()
                                 readX'(k)
                                 )
        btClose.Click.Add (fun _ -> frm.Close()
                                    Application.Exit ())
        frm.Controls.AddRange [| tb; text; btOk; btClose |]
        frm.Show()
    readX' ""
    Application.Run()

read_user_text()