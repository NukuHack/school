const kiir = document.getElementById("kiir");
const page = document.getElementById("page");
//kiir.innerHTML+="apple"
let Data;

// XMLHttpRequest létrehozása
var xhttp = new XMLHttpRequest();

xhttp.onreadystatechange = function(){
    if(this.readyState == 4&& this.status == 200){
        var res = JSON.parse(this.responseText)
        Data = res;
        display(res);
        console.log(res);
        return Data;
    }
}

xhttp.open('GET','https://jsonplaceholder.typicode.com/users',true);

xhttp.send();





function display(Data) {
    for (let i = 0; i < Data.length; i++) {
        const x = Data[i];
        let kiirh = kiir.innerHTML;
        kiirh+=`<div id="out_${x.id}" class="out">`;
        kiirh+=`<p id="out_${x.id}">${x.name}</p>`;
        kiirh+=`<input type="button" id="open_${x.id}" onclick="Open_Page(${x.id})" value="More about ${x.name}"><br>`;
        kiirh+=`</div>`;
        kiir.innerHTML=kiirh;
    }
}

function Open_Page(num) {
    num --;
    // will be called every time the user clicks on a game and will make a separate "webpage" what will only be an overlay on top of everything else

    page.style.visibility="visible";
    let page_out = page.innerHTML;

    page_out = `<div class="full_page">`;
    page_out+= `<p class="game_title">${Data[num].name}</p><br>
                        <div class="small_desc">Email : ${Data[num].email}</div><br>
                        <p class="review">Phone : ${Data[num].phone}</p>`;

    page_out+= `<input type="button" value="Close this page" onclick="Close_Page()">`;
    page_out+= `</div>`;

    page.innerHTML=page_out;
}
function Close_Page() {
    page.style.visibility="hidden";
    page.innerHTML = '';
}
