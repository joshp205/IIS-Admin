"use strict";

var req_per_sec = [];

var net_bytes_sent = [];
var net_bytes_recv = [];

var mem_installed = [];
var mem_in_use = [];

var cpu_sys_perc_use = [];
var cpu_percent_usage = [];
var connection = new signalR.HubConnectionBuilder().withUrl("/pollHub").build();

// Requests Chart
var reqtx = document.getElementById("requestsChart").getContext('2d');
var reqChart = new Chart(reqtx, {
    type: 'line',
    data: {
        datasets: [
            {
                label: 'Requests/sec',
                data: req_per_sec,
                backgroundColor: 'rgba(55, 178, 0, 0.1)',
                borderColor: 'rgb(55, 178, 0)',
                borderWidth: 1,
                pointRadius: 0
            }
        ]
    },
    options: {
        scales: {
            xAxes: [{
                display: false,
                type: 'category',
                labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'],
            }],
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    }
});

// Network Chart
var nettx = document.getElementById("networkChart").getContext('2d');
var netChart = new Chart(nettx, {
    type: 'line',
    data: {
        datasets: [
            {
                label: 'Bytes sent/sec',
                data: net_bytes_sent,
                backgroundColor: 'rgba(55, 178, 0, 0.1)',
                borderColor: 'rgb(55, 178, 0)',
                borderWidth: 1,
                pointRadius: 0
            },
            {
                label: 'Bytes received/sec',
                data: net_bytes_recv,
                backgroundColor: 'rgba(2, 154, 178, 0.1)',
                borderColor: 'rgb(2, 154, 178)',
                borderWidth: 1,
                pointRadius: 0
            }
        ]
    },
    options: {
        scales: {
            xAxes: [{
                display: false,
                type: 'category',
                labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'],
            }],
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    }
});

// Memory Chart
var memtx = document.getElementById("memoryChart").getContext('2d');
var memChart = new Chart(memtx, {
    type: 'line',
    data: {
        datasets: [
            {
                label: 'Total memory installed (GB)',
                data: mem_installed,
                backgroundColor: 'rgba(55, 178, 0, 0.1)',
                borderColor: 'rgb(55, 178, 0)',
                borderWidth: 1,
                pointRadius: 0
            },
            {
                label: 'Memory in use (GB)',
                data: mem_in_use,
                backgroundColor: 'rgba(2, 154, 178, 0.1)',
                borderColor: 'rgb(2, 154, 178)',
                borderWidth: 1,
                pointRadius: 0
            }
        ]
    },
    options: {
        scales: {
            xAxes: [{
                display: false,
                type: 'category',
                labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'],
            }],
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    }
});

// CPU Chart
var cputx = document.getElementById("cpuChart").getContext('2d');
var cpuChart = new Chart(cputx, {
    type: 'line',
    data: {
        datasets: [
            {
                label: 'System percent usage',
                data: cpu_sys_perc_use,
                backgroundColor: 'rgba(255, 99, 132, 0.1)',
                borderColor: 'rgb(255, 99, 132)',
                borderWidth: 1,
                pointRadius: 0
            },
            {
                label: 'Percent usage',
                data: cpu_percent_usage,
                backgroundColor: 'rgba(2, 154, 178, 0.1)',
                borderColor: 'rgb(2, 154, 178)',
                borderWidth: 1,
                pointRadius: 0
            }
        ]
    },
    options: {
        scales: {
            xAxes: [{
                display: false,
                type: 'category',
                labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'],
            }],
            yAxes: [{
                ticks: {
                    max: 100,
                    beginAtZero: true
                }
            }]
        }
    }
});

connection.on("ReceiveMessage", (message) => {

    var monitor = JSON.parse(message);

    req_per_sec.push(monitor.requests.per_sec);

    net_bytes_sent.push(monitor.network.bytes_sent_sec);
    net_bytes_recv.push(monitor.network.bytes_recv_sec);

    mem_installed.push(monitor.memory.installed / 1000000000);
    mem_in_use.push(monitor.memory.system_in_use / 1000000000);

    cpu_sys_perc_use.push(monitor.cpu.system_percent_usage);
    cpu_percent_usage.push(monitor.cpu.percent_usage);


    if (req_per_sec.length > 15) {
        req_per_sec.shift();
    }
    if (net_bytes_sent.length > 15) {
        net_bytes_sent.shift();
    }
    if (net_bytes_recv.length > 15) {
        net_bytes_recv.shift();
    }
    if (mem_installed.length > 15) {
        mem_installed.shift();
    }
    if (mem_in_use.length > 15) {
        mem_in_use.shift();
    }
    if (cpu_sys_perc_use.length > 15) {
        cpu_sys_perc_use.shift();
    }
    if (cpu_percent_usage.length > 15) {
        cpu_percent_usage.shift();
    }

    reqChart.update(0);
    netChart.update(0);
    memChart.update(0);
    cpuChart.update(0);
});

connection.start()
    .then((fulfilled) => {
        connection.invoke("Connect", JSON.stringify(server)).catch((error) => {
            console.error(error.toString());
        })
    })
    .catch((error) => {
        console.error(error.toString());
    });

window.onbeforeunload = function () {
    connection.invoke("Disconnect").catch((error) => {
        console.error(error.toString());
    });

    connection.stop();
}