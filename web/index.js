const kiir = document.getElementById("kiir");
const page = document.getElementById("page");
//kiir.innerHTML+="apple"
let Data;

// XMLHttpRequest létrehozása
var xhttp = new XMLHttpRequest();

xhttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        var res = JSON.parse(this.responseText)
        Data = res;
        display(res);
        console.log(res);
        return Data;
    }
}

xhttp.open('GET', 'https://jsonplaceholder.typicode.com/users', true);
/*
xhttp.send();
*/

function display(Data) {
    let table_help = "<table><tbody>";
    table_help += "<th>Name</th><th>Username</th><th>E-mail</th><th>Phone</th><th>More</th>";
    for (let i = 0; i < Data.length; i++) {
        const x = Data[i];
        table_help += `<tr>`;
        table_help += `<td class="out">
            <p id="out_name_${x.id}">${x.name}</p>
        </td>`;
        table_help += `<td class="out">
            <p id="out_username_${x.id}">${x.username}</p>
        </td>`;
        table_help += `<td class="out">
            <p id="out_email_${x.id}">${x.email}</p>
        </td>`;
        table_help += `<td class="out">
            <p id="out_phone_${x.id}">${x.phone}</p>
        </td>`;
        table_help += `<td>
            <input type="button" id="out_button_${x.id}" onclick="Open_Page(${x.id})" value="More" class="out_button">
        </td>`;
        table_help += `</tr>`;

    }
    table_help += "</tbody></table>";
    kiir.insertAdjacentHTML('beforeend', table_help);
}


function Open_Page(num) {
    num--;
    // will be called every time the user clicks on a game and will make a separate "webpage" what will only be an overlay on top of everything else

    page.style.visibility = "visible";
    let page_out = `<div class="full_page">`;
    page_out += `
        <p class="game_title">${Data[num].name}</p>
        <p>Email : ${Data[num].email}</p>
        <p>Phone : ${Data[num].phone}</p>
        <p>City : ${Data[num].address.city}</p>
        <p>Address : ${Data[num].address.street}</p>
        <p>Door : ${Data[num].address.suite}</p>
    `;
    page_out += `<input type="button" value="Close this page" onclick="Close_Page()">`;
    page_out += `</div>`;

    page.innerHTML=page_out;
}

function Close_Page() {
    page.style.visibility = "hidden";
}



