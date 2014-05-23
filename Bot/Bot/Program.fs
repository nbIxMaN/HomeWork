module FormExample

open System.Windows.Forms
open System.Drawing
open AIMLbot

let bot_loading = 
    let bot = new Bot()
    bot.loadSettings()
    let user = new User("user", bot)
    bot.isAcceptingUserInput <- false
    bot.loadAIMLFromFiles()
    bot.isAcceptingUserInput <- true
    bot

let user_loading bot = new User("user", bot)

let bot_talking user bot text = 
    let r = new Request (text, user, bot)
    let res = bot.Chat(r)
    res.Output

let read_user_text() =
    let bot = bot_loading
    let user = user_loading (bot)
    let tb = new TextBox(Top = 400)
    tb.Size <- new Size(780, 100) 
    let text = new TextBox()
    text.Size <- new Size(780, 380)
    text.Multiline <- true
    text.WordWrap <- true
    text.ReadOnly <- true
    text.ScrollBars <- ScrollBars.Vertical
    let form = new Form(Visible=true, Text = "My chat with bot")
    form.MaximizeBox <- false
    form.Size <- new Size(800, 600)
    form.FormBorderStyle <- FormBorderStyle.FixedDialog
    form.Icon <- new Icon ("Значок3.ico")
    let btOk = new Button(Text="Send", Top = 500)
    let btClose = new Button(Text="Close", Top = 500, Left = 100)
    form.Controls.AddRange [| tb; text; btOk; btClose |]
    let SendMessage() = 
        let x = tb.Text
        if not (x.Trim() = "") then
            text.AppendText("Me: " + x + "\n")
            let k = bot_talking user bot x
            text.AppendText("Bot: " + k + "\n")
            tb.Clear()
            tb.Focus() |> ignore
    let CloseChat() =
        form.Close()
        Application.Exit ()
    tb.Focus() |> ignore
    btOk.Click.Add (fun _ -> SendMessage())
    btClose.Click.Add (fun _ -> CloseChat())
    tb.KeyDown.Add(fun x -> if x.KeyCode = Keys.Enter then SendMessage())
    text.KeyDown.Add(fun x -> if x.KeyCode = Keys.Enter then SendMessage())
    form.KeyDown.Add(fun x -> if x.KeyCode = Keys.Enter then SendMessage())
    tb.KeyDown.Add(fun x -> if x.KeyCode = Keys.Escape then CloseChat())
    text.KeyDown.Add(fun x -> if x.KeyCode = Keys.Escape then CloseChat())
    form.KeyDown.Add(fun x -> if x.KeyCode = Keys.Escape then CloseChat())
    Application.Run(form)

read_user_text()